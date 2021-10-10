using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class ParkingService : MultitanentService, IParkingService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IParkingLaneService parkingLaneService;
        private readonly ISlotTypeConfigurationService slotConfigService;

        public ParkingService(ApplicationDbContext dbContext, 
            IParkingLaneService parkingLaneService,
            ISlotTypeConfigurationService slotConfigService,
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.parkingLaneService = parkingLaneService;
            this.slotConfigService = slotConfigService;
        }

        public async Task<ParkingViewModel> CreateParking(CreateUpdateParkingViewModel model)
        {
            ParkingConfig parking = new()
            {
                Name = model.Name,
                Address = model.Address,
                ClientId = GetClientId()
            };
            var result = await dbContext.AddAsync(parking);
            await dbContext.SaveChangesAsync();
            return new ParkingViewModel()
            {
                Id = result.Entity.Id,
                Address = result.Entity.Address,
                Name = result.Entity.Name,
                NumberOfLanes = 0,
                NumberOfLots = 0
            };
        }

        public async Task<QueryResultModel<ParkingViewModel>> GetParkings(QueryModel query)
        {
            var dataQuery = dbContext.Parkings
                .Include(x => x.ParkingLanes)
                .Include(x=>x.SlotTypeConfigurations)
                .Where(x => x.ClientId == ClientId);
            var totalCount = await dataQuery.CountAsync();
            var page = query.Page;
            var data = await dataQuery
                .OrderBy(x=>x.Name)
                .PagedBy(query.Page, query.PageSize)
                .Select(x=> new ParkingViewModel() {
                    Id = x.Id,
                    Address = x.Address,
                    Name = x.Name,
                    NumberOfLanes = x.ParkingLanes.Count,
                    NumberOfLots = x.SlotTypeConfigurations.Sum(y=>y.SlotCount)
                }).ToListAsync();
            return new QueryResultModel<ParkingViewModel>(data)
            {
                Page = page,
                TotalCount = totalCount
            };
        }

        public async Task<ParkingDetailsModel> GetParkingById(Guid parkingId)
        {
            var parking = await dbContext.Parkings
                .SingleAsync(x => x.ClientId == ClientId && x.Id == parkingId );
            var parkingLanes = await parkingLaneService.GetParkingLanes(parkingId);
            var parkingSlot = await slotConfigService.GetSlotTypeConfigs(parkingId);
            return new ParkingDetailsModel
            {
                Id = parking.Id,
                Name = parking.Name,
                Address = parking.Address,
                ParkingLanes = parkingLanes.ToList(),
                SlotTypeConfigs = parkingSlot.ToList()
            };
        }

        public async Task<ParkingViewModel> UpdateParking(CreateUpdateParkingViewModel model)
        {
            var parking = await dbContext.Parkings
               .Where(x => x.ClientId == ClientId && x.Id == model.Id)
               .FirstOrDefaultAsync();
            if (parking != null)
            {
                parking.Name = model.Name;
                parking.Address = model.Address;
                dbContext.Update(parking);
                await dbContext.SaveChangesAsync(); 
            }
            return new ParkingViewModel()
            {
                Id = parking.Id,
                Address = parking.Address,
                Name = parking.Name
            };
        }

        public async Task<int> AssignCards(CardParkingAssignmentViewModel assignment)
        {
            var duplicateIds = await dbContext.CardParkingAssignments
                .Where(x => x.ParkingId == assignment.ParkingId && assignment.CardsId.Contains(x.CardId))
                .Select(x => x.CardId)
                .ToListAsync();
            var assignmentList = assignment.CardsId
                .Where(x => !duplicateIds.Contains(x))
                .Select(x =>
                new CardParkingAssignment()
                {
                    CardId = x,
                    ParkingId = assignment.ParkingId
                });

            await dbContext.CardParkingAssignments.AddRangeAsync(assignmentList);
            var rows = await dbContext.SaveChangesAsync();
            return rows;
        }

        public async Task<int> RemoveCards(CardParkingAssignmentViewModel assignment)
        {
            var assignments = await dbContext.CardParkingAssignments
                .Where(x => x.ParkingId == assignment.ParkingId && assignment.CardsId.Contains(x.CardId))
                .ToListAsync();
            dbContext.CardParkingAssignments.RemoveRange(assignments);
            var rows = await dbContext.SaveChangesAsync();
            return rows;
        }

        public async Task<IEnumerable<ParkingViewModel>> ImportData(IEnumerable<ParkingDataImport> data)
        {
            var parkings = data.Select(model =>
            {
                ParkingConfig parking = new()
                {
                    Name = model.Name,
                    Address = model.Address,
                    ClientId = GetClientId(),
                };
                return parking;
            });
            await dbContext.AddRangeAsync(parkings);
            await dbContext.SaveChangesAsync();
            return parkings.Select(x => new ParkingViewModel()
            {
                Id = x.Id,
                Address = x.Address,
                Name = x.Name,
                NumberOfLanes = 0,
                NumberOfLots = 0
            });
        }
    }
}

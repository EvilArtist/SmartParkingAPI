using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class ParkingService : IParkingService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IParkingLaneService parkingLaneService;
        private readonly ISlotTypeConfigurationService slotConfigService;

        public ParkingService(ApplicationDbContext dbContext, 
            IMapper mapper, 
            IParkingLaneService parkingLaneService,
            ISlotTypeConfigurationService slotConfigService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.parkingLaneService = parkingLaneService;
            this.slotConfigService = slotConfigService;
        }
        public async Task<ParkingViewModel> CreateParking(CreateUpdateParkingViewModel model)
        {
            ParkingConfig parking = new()
            {
                Name = model.Name,
                Address = model.Address,
                ClientId = model.ClientId
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
                .Where(x => x.ClientId == query.ClientId);
            var totalCount = await dataQuery.CountAsync();
            var page = query.Page;
            var data = await dataQuery.PagedBy(query.Page, query.PageSize)
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

        public async Task<ParkingDetailsModel> GetParkingById(string clientId, Guid parkingId)
        {
            var parking = await dbContext.Parkings
                .SingleAsync(x => x.ClientId == clientId && x.Id == parkingId );
            var parkingLanes = await parkingLaneService.GetParkingLanes(parkingId);
            var parkingSlot = await slotConfigService.GetSlotTypeConfigs(clientId, parkingId);
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
               .Where(x => x.ClientId == model.ClientId && x.Id == model.Id)
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
    }
}

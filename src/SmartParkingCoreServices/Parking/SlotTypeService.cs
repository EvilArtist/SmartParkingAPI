using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class SlotTypeService : MultitanentService, ISlotTypeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public SlotTypeService(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper) : base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<SlotTypeViewModel> CreateSlotTypeAsync(SlotTypeViewModel model)
        {
            var slotType = mapper.Map<SlotType>(model);
            var newSlotType = await dbContext.AddAsync(slotType);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SlotTypeViewModel>(newSlotType.Entity);
        }

        public async Task<SlotTypeViewModel> GetSlotTypeByIdAsync(Guid id)
        {
            var slotType = await dbContext.SlotTypes
               .Where(x => x.Id == id && x.ClientId == ClientId)
               .FirstOrDefaultAsync();
            return mapper.Map<SlotTypeViewModel>(slotType);
        }

        public async Task<IEnumerable<SlotTypeViewModel>> SearchSlotTypesAsync()
        {
            var slotTypes = await dbContext.SlotTypes
                .Where(x => x.ClientId == ClientId)
                .ToListAsync();

            return mapper.Map<List<SlotType>, List<SlotTypeViewModel>>(slotTypes);
        }

        public async Task<bool> DeleteSlotTypeAsync(EntityDeleteViewModel deleteViewModel)
        {
            var slotType = await dbContext.SlotTypes
                   .Where(x => x.ClientId == ClientId && x.Id == deleteViewModel.Id)
                   .FirstOrDefaultAsync();
            dbContext.SlotTypes.Remove(slotType);
            int row = await dbContext.SaveChangesAsync();
            return row >= 0;
        }

        public async Task<SlotTypeViewModel> UpdateSlotTypeAsync(SlotTypeViewModel model)
        {
            var slotType = await dbContext.SlotTypes
                   .Where(x => x.ClientId == model.ClientId && x.Id == model.Id)
                   .FirstOrDefaultAsync();
            if (slotType != null)
            {
                slotType = mapper.Map(model, slotType);
                dbContext.Update(slotType);
                await dbContext.SaveChangesAsync();
                return model;
            } else
            {
                throw new DbUpdateException("No data found");
            }
        }

        public async Task<IEnumerable<SlotTypeViewModel>> GetSlotTypesAvailableAsync(Guid parkingId)
        {
            var configuredSlotTypes = await dbContext.SlotTypeConfigurations
                   .Where(x => x.ClientId == ClientId && x.ParkingId == parkingId)
                   .Select(x => x.SlotTypeId)
                   .ToListAsync();
            var availableSlotTypes = await dbContext.SlotTypes
                .Where(x => x.ClientId == ClientId && !configuredSlotTypes.Contains(x.Id))
                .ToListAsync();
            return mapper.Map<List<SlotType>, List<SlotTypeViewModel>>(availableSlotTypes);
        }

        public async Task<IEnumerable<SlotTypeViewModel>> ImportData(IEnumerable<SlotTypeDataImport> data)
        {
            var slotTypeNameList = data.Select(x => x.SlotName);
            var updateSlotTypes = await dbContext.SlotTypes
                    .Where(slot => slotTypeNameList.Contains(slot.SlotName) && slot.ClientId == ClientId)
                    .ToListAsync();
            foreach (var slotType in updateSlotTypes)
            {
                var dataModel = data.First(x => x.SlotName == slotType.SlotName);
                mapper.Map(dataModel, slotType);
                dbContext.Update(slotType);
            }
            List<SlotType> newSlotTypes = new();
            foreach (var slotType in data.Where(x=> !updateSlotTypes.Any(y=>y.SlotName == x.SlotName)))
            {
                var newSlotType = mapper.Map<SlotType>(slotType);
                await dbContext.AddAsync(newSlotType);
                newSlotTypes.Add(newSlotType);
            }
            await dbContext.SaveChangesAsync();
            return mapper.Map<IEnumerable<SlotType>, IEnumerable<SlotTypeViewModel>>(updateSlotTypes.Union(newSlotTypes));
        }
    }
}

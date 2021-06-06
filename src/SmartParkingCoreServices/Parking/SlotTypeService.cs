using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.SlotType;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class SlotTypeService : ISlotTypeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public SlotTypeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<SlotTypeViewModel> CreateSlotTypeAsync(SlotTypeViewModel model)
        {
            var slotType = mapper.Map<SlotType>(model);
            var newSlotType = await dbContext.AddAsync(slotType);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SlotTypeViewModel>(newSlotType);
        }

        public async Task<SlotTypeViewModel> GetSlotTypeByIdAsync(Guid id, string clientId)
        {
            var slotType = await dbContext.SlotTypes
               .Where(x => x.Id == id && x.ClientId == clientId)
               .FirstOrDefaultAsync();
            return mapper.Map< SlotTypeViewModel>(slotType);
        }

        public async Task<IEnumerable<SlotTypeViewModel>> SearchSlotTypesAsync(string clientId)
        {
            var slotTypes = await dbContext.SlotTypes
                .Where(x => x.ClientId == clientId)
                .ToListAsync();

            return mapper.Map<List<SlotType>, List<SlotTypeViewModel>>(slotTypes);
        }

        public async Task<bool> DeleteSlotTypeAsync(EntityDeleteViewModel deleteViewModel)
        {
            var slotType = await dbContext.SlotTypes
                   .Where(x => x.ClientId == deleteViewModel.ClientId && x.Id == deleteViewModel.Id)
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
    }
}

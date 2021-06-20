using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
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
    public class SlotTypeConfigurationService : ISlotTypeConfigurationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public SlotTypeConfigurationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SlotTypeConfigViewModel>> CreateOrUpdateSlotTypeConfigs(UpdateSlotTypeConfigsViewModel model)
        {
            var currentConfig = await dbContext.SlotTypeConfigurations
                .Where(x => x.ClientId == model.ClientId && x.ParkingId == model.ParkingId)
                .ToListAsync();
            for (int i = 0; i < currentConfig.Count; i++)
            {
                var config = currentConfig[i];
                var findConfig = model.SlotTypeConfigs.FirstOrDefault(x => x.SlotTypeId == config.SlotTypeId);
                if (findConfig is null)
                {
                    dbContext.SlotTypeConfigurations.Remove(config);
                } 
                else
                {
                    currentConfig[i] = mapper.Map(findConfig, config);
                    dbContext.Update(currentConfig[i]);
                }
            }

            var newConfigs = model.SlotTypeConfigs.Where(config => !currentConfig.Any(x => x.SlotTypeId == config.SlotTypeId)).ToList();
            await dbContext.AddRangeAsync(mapper.Map<IEnumerable<UpdateSlotTypeConfigViewModel>, IEnumerable<SlotTypeConfiguration>>(newConfigs));
            await dbContext.SaveChangesAsync();
            currentConfig = await dbContext.SlotTypeConfigurations
                .Where(x => x.ClientId == model.ClientId && x.ParkingId == model.ParkingId)
                .ToListAsync();
            return mapper.Map<List<SlotTypeConfiguration>, List<SlotTypeConfigViewModel>>(currentConfig);

        }

        public async Task<IEnumerable<SlotTypeConfigViewModel>> GetSlotTypeConfigs(string clientId, Guid parkingId)
        {
            var query = dbContext.SlotTypeConfigurations.Where(x => x.ClientId == clientId && x.ParkingId == parkingId);
            var result = await query.ToListAsync();
            return mapper.Map<List<SlotTypeConfiguration>, List<SlotTypeConfigViewModel>>(result);
        }
    }
}

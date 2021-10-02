using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.DataImport;
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
    public class SlotTypeConfigurationService : MultitanentService, ISlotTypeConfigurationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public SlotTypeConfigurationService(ApplicationDbContext dbContext, 
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper) : base(httpContextAccessor)
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
            var query = dbContext.SlotTypeConfigurations
                .Include(x=>x.SlotType)
                .Where(x => x.ClientId == clientId && x.ParkingId == parkingId);
            var result = await query.ToListAsync();
            return mapper.Map<List<SlotTypeConfiguration>, List<SlotTypeConfigViewModel>>(result);
        }

        public async Task<SlotTypeConfigViewModel> CreateOrUpdateSlotTypeConfig(SlotTypeConfigViewModel model)
        {
            var slotType = await dbContext.SlotTypeConfigurations
                .Where(x => x.ClientId == ClientId && 
                    x.ParkingId == model.ParkingId &&
                    x.SlotTypeId == model.SlotTypeId)
                .FirstOrDefaultAsync();
            if (slotType != null)
            {
                slotType.SlotCount = model.SlotCount;
                dbContext.Update(slotType);
            } 
            else
            {
                slotType = mapper.Map<SlotTypeConfiguration>(model);
                await dbContext.AddAsync(slotType);
            }
            await dbContext.SaveChangesAsync();
            return mapper.Map<SlotTypeConfigViewModel>(slotType);
        }

        public async Task<IEnumerable<SlotTypeConfigViewModel>> ImportData(IEnumerable<SlotTypeConfigDataImport> data)
        {
            var slotTypesName = data.Select(x => x.SlotName).Distinct();
            var parkingsName = data.Select(x => x.ParkingName).Distinct();
            var slotTypes = await dbContext.SlotTypes
                .Where(x => x.ClientId == ClientId && slotTypesName.Contains(x.SlotName))
                .ToListAsync(); ;
            var parkings = await dbContext.Parkings
                .Where(x => x.ClientId == ClientId && parkingsName.Contains(x.Name))
                .ToListAsync();
            var slotTypeConfigs = new List<SlotTypeConfiguration>();
            var notFoundParkings = parkingsName.Where(x => !parkings.Any(y => y.Name == x));
            var notFoundSlotTypes = slotTypesName.Where(x => !slotTypes.Any(y => y.SlotName == x));
            foreach (var item in notFoundParkings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Not found parking " + item);
                Console.ResetColor();
            }
            foreach (var item in notFoundSlotTypes)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Not found SlotType " + item);
                Console.ResetColor();
            }
            foreach (var parking in parkings)
            {
                foreach (var slotType in slotTypes)
                {
                    var slotTypeConfig = await dbContext.SlotTypeConfigurations
                        .Where(x => x.ClientId == ClientId &&
                            x.ParkingId == parking .Id &&
                            x.SlotTypeId == slotType.Id)
                        .FirstOrDefaultAsync();
                    var slotCount = data.First(x => x.SlotName == slotType.SlotName && x.ParkingName == parking.Name).NumberOfSlot;
                    if(slotTypeConfig != null)
                    {
                        slotTypeConfig.SlotCount = slotCount;
                        dbContext.Update(slotTypeConfig);
                    } 
                    else
                    {
                        slotTypeConfig = new SlotTypeConfiguration()
                        {
                            SlotTypeId = slotType.Id,
                            ParkingId = parking.Id,
                            SlotCount = slotCount
                        };
                        await dbContext.AddAsync(slotTypeConfig);
                    }
                    slotTypeConfigs.Add(slotTypeConfig);
                }
            }
            await dbContext.SaveChangesAsync();
            return mapper.Map<List<SlotTypeConfiguration>, List<SlotTypeConfigViewModel>>(slotTypeConfigs);
        }
    }
}

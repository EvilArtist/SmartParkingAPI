using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
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
    public class VehicleTypeService : MultitanentService, IVehicleTypeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public VehicleTypeService(ApplicationDbContext dbContext, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<VehicleTypeViewModel> CreateVehicleType(CreateUpdateVehicleTypeViewModel model)
        {
            var vehicleType = mapper.Map<VehicleType>(model);
            var result = await dbContext.AddAsync(vehicleType);
            await dbContext.SaveChangesAsync();
            return mapper.Map<VehicleTypeViewModel>(result.Entity);
        }

        public async Task<VehicleTypeViewModel> GetVehicleTypeById(Guid id)
        {
            var vehicleType = await dbContext.VehicleTypes
               .Where(x => x.ClientId == ClientId && x.Id == id)
               .Select(x => new VehicleTypeViewModel()
               {
                   SlotTypeId = x.SlotTypeId,
                   Id = x.Id,
                   SlotTypeName = x.SlotType.SlotName,
                   CardCount = x.Cards == null ? 0 : x.Cards.Count,
                   Description = x.Description,
                   Name = x.Name
               })
               .FirstOrDefaultAsync();
            return vehicleType;
        }

        public async Task<IEnumerable<VehicleTypeViewModel>> GetVehicleTypes()
        {
            var vehicleTypes = await dbContext.VehicleTypes
                .Where(x => x.ClientId == ClientId)
                .Select (x=> new VehicleTypeViewModel() { 
                    SlotTypeId = x.SlotTypeId,
                    Id = x.Id,
                    SlotTypeName = x.SlotType.SlotName,
                    CardCount = x.Cards == null ? 0: x.Cards.Count,
                    Description = x.Description,
                    Name = x.Name
                })
                .ToListAsync();
            return vehicleTypes;
        }

        public async Task<VehicleTypeViewModel> UpdateVehicleType(CreateUpdateVehicleTypeViewModel model)
        {
            var vehicleType = await dbContext.VehicleTypes
                .Where(x => x.ClientId == ClientId && x.Id == model.Id)
                .FirstOrDefaultAsync();
            mapper.Map(model, vehicleType);
            var result = dbContext.Update(vehicleType);
            await dbContext.SaveChangesAsync();
            return mapper.Map<VehicleTypeViewModel>(result.Entity);
        }
    }
}

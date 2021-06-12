using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.Parking.CameraConfiguration;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class CameraService : ICameraConfigService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public CameraService(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<CameraConfigurationViewModel> CreateCameraAsync(CameraConfigurationViewModel model)
        {
            var camera = mapper.Map<CameraConfiguration>(model);
            var result = await dbContext.AddAsync(camera);
            await dbContext.SaveChangesAsync();
            return mapper.Map<CameraConfigurationViewModel>(result.Entity);
        }

        public async Task<CameraConfigurationViewModel> GetCameraById(Guid id, string clientId)
        {
            var camera = await dbContext.CameraConfigurations.Where(x => x.ClientId == clientId
                  && x.Id == id
              ).FirstOrDefaultAsync();
            return mapper.Map<CameraConfigurationViewModel>(camera);
        }

        public async Task<IEnumerable<CameraConfigurationViewModel>> GetCamerasAsync(string clientId, DeviceStatus? status)
        {
            var query = dbContext.CameraConfigurations.Where(x => x.ClientId == clientId);
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
            var cameras = await query.ToListAsync();
            return mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(cameras);
        }

        public async Task<IEnumerable<CameraProtocolTypeViewModel>> GetCameraProtocols()
        {
            var protocols = await dbContext.CameraProtocolType.ToListAsync();
            return mapper.Map<List<CameraProtocolType>, List<CameraProtocolTypeViewModel>>(protocols);
        }
    }
}

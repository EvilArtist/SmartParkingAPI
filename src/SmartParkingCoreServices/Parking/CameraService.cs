using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
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
    public class CameraService : MultitanentService, ICameraConfigService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public CameraService(IMapper mapper, 
            ApplicationDbContext dbContext, 
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
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

        public async Task<CameraConfigurationViewModel> GetCameraById(Guid id)
        {
            var camera = await dbContext.CameraConfigurations
                .Where(x => x.ClientId == ClientId && x.Id == id)
                .FirstOrDefaultAsync();
            return mapper.Map<CameraConfigurationViewModel>(camera);
        }

        public async Task<IEnumerable<CameraConfigurationViewModel>> GetCamerasAsync(DeviceStatus? status)
        {
            var query = dbContext.CameraConfigurations.Where(x => x.ClientId == ClientId);
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

        public async Task<IEnumerable<CameraConfigurationViewModel>> ImportData(IEnumerable<CameraImportData> data)
        {
            var protocols = await dbContext.CameraProtocolType.ToListAsync();
            var cameraNameList = data.Select(x => x.CameraName);
            var updateCameras = await dbContext.CameraConfigurations
                .Where(x => cameraNameList.Contains(x.CameraName))
                .ToListAsync();
            foreach (var camera in updateCameras)
            {
                var model = data.FirstOrDefault(x => x.CameraName == camera.CameraName);
                mapper.Map(model, camera);
            }
            dbContext.UpdateRange(updateCameras);
            var newCameras = new List<CameraConfiguration>();
                
            foreach(var cameraData in data.Where(y => !updateCameras.Any(x => x.CameraName == y.CameraName)))
            {
                var newCamera = mapper.Map<CameraConfiguration>(cameraData);
                var protocol = protocols.FirstOrDefault(x => x.ProtocolName.ToLower() == cameraData.Protocol.ToLower());
                if(protocol != null)
                {
                    newCamera.ProtocolId = protocol.Id;
                    newCameras.Add(newCamera);
                }
            }

            await dbContext.AddRangeAsync(newCameras);
            await dbContext.SaveChangesAsync();
            return mapper.Map<List<CameraConfiguration>, List<CameraConfigurationViewModel>>(newCameras.Union(updateCameras).ToList());
        }
    }
}

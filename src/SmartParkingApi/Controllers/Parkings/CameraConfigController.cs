using Microsoft.AspNetCore.Mvc;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.CameraConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraConfigController : ControllerBase
    {
        private readonly ICameraConfigService service;

        public CameraConfigController(ICameraConfigService service)
        {
            this.service = service;
        }

        [HttpGet()]
        public async Task<ServiceResponse<IEnumerable<CameraConfigurationViewModel>>> GetCameras(string status)
        {
            try
            {
                var clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                DeviceStatus? deviceStatus = null;
                if (!string.IsNullOrEmpty(status))
                {
                    bool canParse = Enum.TryParse(status, out DeviceStatus tempDeviceStatus);
                    if (canParse)
                    {
                        deviceStatus = tempDeviceStatus;
                    }
                }
                var result = await service.GetCamerasAsync(clientId, deviceStatus);

                return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Fail(e);
            }
        }

        [HttpPost()]
        public async Task<ServiceResponse<CameraConfigurationViewModel>> CreateCameras(CameraConfigurationViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await service.CreateCameraAsync(model);
                return ServiceResponse<CameraConfigurationViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CameraConfigurationViewModel>.Fail(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<CameraConfigurationViewModel>> GetConfigById(Guid id)
        {
            try
            {
                var clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await service.GetCameraById(id, clientId);
                return ServiceResponse<CameraConfigurationViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CameraConfigurationViewModel>.Fail(e);
            }
        }

        [HttpGet("protocols")]
        public async Task<ServiceResponse<IEnumerable<CameraProtocolTypeViewModel>>> GetProtocols()
        {
            try
            {
                var result = await service.GetCameraProtocols();
                return ServiceResponse<IEnumerable<CameraProtocolTypeViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<CameraProtocolTypeViewModel>>.Fail(e);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.SerialPortConfiguration;
using static SmartParking.Share.Constants.IdentityConstants;
using SmartParking.Share.Constants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialPortConfigurationController : ControllerBase
    {
        private readonly ISerialPortService serialPortService;

        public SerialPortConfigurationController(ISerialPortService serialPortService)
        {
            this.serialPortService = serialPortService;
        }

        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<SerialPortConfigViewModel>>> GetConfig(string status)
        {
            try
            {
                var clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                DeviceStatus? deviceStatus = null;
                if (!string.IsNullOrEmpty(status))
                {
                    bool canParse = Enum.TryParse<DeviceStatus>(status, out DeviceStatus tempDeviceStatus);
                    if (canParse)
                    {
                        deviceStatus = tempDeviceStatus;
                    }
                }
                var result = await serialPortService.SearchSerialPortConfig(clientId, deviceStatus);
                return ServiceResponse<IEnumerable<SerialPortConfigViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<SerialPortConfigViewModel>>.Fail(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<SerialPortConfigViewModel>> GetConfigById(Guid id)
        {
            try
            {
                var clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await serialPortService.GetSerialPortConfigById(id, clientId);
                return ServiceResponse<SerialPortConfigViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SerialPortConfigViewModel>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<SerialPortConfigViewModel>> CreateConfig(SerialPortConfigViewModel configurationViewModel)
        {
            try
            {
                configurationViewModel.GetClientIdFromContext(HttpContext);
                var result = await serialPortService.CreateConfig(configurationViewModel);
                return ServiceResponse<SerialPortConfigViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SerialPortConfigViewModel>.Fail(e);
            }
        }

    }
}

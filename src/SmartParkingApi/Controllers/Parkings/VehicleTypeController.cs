using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeService vehicleTypeService;

        public VehicleTypeController(IVehicleTypeService vehicleTypeService)
        {
            this.vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<VehicleTypeViewModel>>> GetVehicleTypes()
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await vehicleTypeService.GetVehicleTypes(clientId);
                return ServiceResponse<IEnumerable<VehicleTypeViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<VehicleTypeViewModel>>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<VehicleTypeViewModel>> CreateVehicleType(CreateUpdateVehicleTypeViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await vehicleTypeService.CreateVehicleType(model);
                return ServiceResponse<VehicleTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<VehicleTypeViewModel>.Fail(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<VehicleTypeViewModel>> GetVehicleTypeById(Guid id)
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await vehicleTypeService.GetVehicleTypeById(clientId, id);
                return ServiceResponse<VehicleTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<VehicleTypeViewModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<VehicleTypeViewModel>> UpdateVehicleType(CreateUpdateVehicleTypeViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await vehicleTypeService.UpdateVehicleType(model);
                return ServiceResponse<VehicleTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<VehicleTypeViewModel>.Fail(e);
            }
        }
    }
}

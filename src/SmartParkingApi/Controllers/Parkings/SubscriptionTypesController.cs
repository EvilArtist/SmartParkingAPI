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
    public class SubscriptionTypesController : ControllerBase
    {
        private readonly ISubscriptionTypeService subscriptionService;

        public SubscriptionTypesController(ISubscriptionTypeService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<SubscriptionTypeViewModel>>> GetVehicleTypes()
        {
            try
            {
                var result = await subscriptionService.GetSubscriptionTypes();
                return ServiceResponse<IEnumerable<SubscriptionTypeViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<SubscriptionTypeViewModel>>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<SubscriptionTypeViewModel>> CreateVehicleType(CreateUpdateSubscriptionTypeViewModel model)
        {
            try
            {
                var result = await subscriptionService.CreateSubscriptionType(model);
                return ServiceResponse<SubscriptionTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SubscriptionTypeViewModel>.Fail(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<SubscriptionTypeViewModel>> GetVehicleTypeById(Guid id)
        {
            try
            {
                var result = await subscriptionService.GetSubscriptionTypeById(id);
                return ServiceResponse<SubscriptionTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SubscriptionTypeViewModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<SubscriptionTypeViewModel>> UpdateVehicleType(CreateUpdateSubscriptionTypeViewModel model)
        {
            try
            {
                var result = await subscriptionService.UpdateSubscriptionType(model);
                return ServiceResponse<SubscriptionTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SubscriptionTypeViewModel>.Fail(e);
            }
        }
    }
}

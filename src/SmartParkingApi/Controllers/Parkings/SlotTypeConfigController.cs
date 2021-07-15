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
    public class SlotConfigController : ControllerBase
    {
        private readonly ISlotTypeConfigurationService service;

        public SlotConfigController(ISlotTypeConfigurationService service)
        {
            this.service = service;
        }

        // GET: api/SlotTypes
        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>> GetSlotTypeConfig(Guid parkingId)
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await service.GetSlotTypeConfigs(clientId, parkingId);
                return ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>.Fail(e);
            }
        }

        [HttpPost]
        public async Task<ServiceResponse<SlotTypeConfigViewModel>> CreateUpdateSlottypeConfig(SlotTypeConfigViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await service.CreateOrUpdateSlotTypeConfig(model);
                return ServiceResponse<SlotTypeConfigViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<SlotTypeConfigViewModel>.Fail(e);
            }
        }
    }
}

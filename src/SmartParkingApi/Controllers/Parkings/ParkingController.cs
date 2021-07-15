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
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService parkingService;

        public ParkingController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }

        [HttpPost("search")]
        public async Task<ServiceResponse<QueryResultModel<ParkingViewModel>>> SearchParking(QueryModel queryModel)
        {
            try
            {
                queryModel.GetClientIdFromContext(HttpContext);
                var result = await parkingService.GetParkings(queryModel);
                return ServiceResponse<QueryResultModel<ParkingViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<QueryResultModel<ParkingViewModel>>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<ParkingViewModel>> CreateParking(CreateUpdateParkingViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await parkingService.CreateParking(model);
                return ServiceResponse<ParkingViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingViewModel>.Fail(e);
            }
        }

        [HttpGet("{parkingId}")]
        public async Task<ServiceResponse<ParkingDetailsModel>> GetParkingById(Guid parkingId)
        {
            try
            {
                var clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
                var result = await parkingService.GetParkingById(clientId, parkingId);
                return ServiceResponse<ParkingDetailsModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingDetailsModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<ParkingViewModel>> UpdateParking(CreateUpdateParkingViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await parkingService.UpdateParking(model);
                return ServiceResponse<ParkingViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingViewModel>.Fail(e);
            }
        }
    }
}

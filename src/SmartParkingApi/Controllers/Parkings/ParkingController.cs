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
                var result = await parkingService.GetParkingById(parkingId);
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
                var result = await parkingService.UpdateParking(model);
                return ServiceResponse<ParkingViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingViewModel>.Fail(e);
            }
        }

        [HttpPost("cards/assign")]
        public async Task<ServiceResponse<int>> AssignCardsToParking(CardParkingAssignmentViewModel model) 
        {
            try
            {
                var result = await parkingService.AssignCards(model);
                return ServiceResponse<int>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<int>.Fail(e);
            }
        }

        [HttpPost("cards/unassign")]
        public async Task<ServiceResponse<int>> UnassignCardsFromParking(CardParkingAssignmentViewModel model)
        {
            try
            {
                var result = await parkingService.RemoveCards(model);
                var response = ServiceResponse<int>.Success(result);
                if (result != model.CardsId.Count())
                {
                    response.Errors = new List<ServiceError>()
                    {
                        new ServiceError()
                        {
                            ErrorCode="CARD_COUNT_NOT_MATCHED",
                            ErrorMessage = "Một số thẻ không được tìm thấy để xoá"
                        }
                    };
                }
                return response;
            }
            catch (Exception e)
            {
                return ServiceResponse<int>.Fail(e);
            }
        }
    }
}

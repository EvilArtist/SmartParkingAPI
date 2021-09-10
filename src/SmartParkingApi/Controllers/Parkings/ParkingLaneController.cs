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
    public class ParkingLaneController : ControllerBase
    {
        private readonly IParkingLaneService parkingLaneService;

        public ParkingLaneController(IParkingLaneService parkingLaneService)
        {
            this.parkingLaneService = parkingLaneService;
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<ParkingLaneViewModel>> CreateParkingLane(CreateUpdateParkingLaneViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await parkingLaneService.CreateParkingLane(model);
                return ServiceResponse<ParkingLaneViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingLaneViewModel>.Fail(e);
            }
        }

        [HttpGet("{laneId}")]
        public async Task<ServiceResponse<ParkingLaneViewModel>> GetParkingLaneById(Guid laneId)
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
               
                var result = await parkingLaneService.GetParkingLaneById(clientId, laneId);
                return ServiceResponse<ParkingLaneViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingLaneViewModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<ParkingLaneViewModel>> UpdateParkingLane(CreateUpdateParkingLaneViewModel model)
        {
            try
            {
                model.GetClientIdFromContext(HttpContext);
                var result = await parkingLaneService.UpdateParkingLane(model);
                return ServiceResponse<ParkingLaneViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingLaneViewModel>.Fail(e);
            }
        }

        [HttpPost("delete/{laneId}")]
        public async Task<ServiceResponse<ParkingLaneViewModel>> DeleteParkingLane(Guid laneId)
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;

                var result = await parkingLaneService.DeleteParkingLane(clientId, laneId);
                return ServiceResponse<ParkingLaneViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingLaneViewModel>.Fail(e);
            }
        }


        [HttpGet("parking/{parkingId}")]
        public async Task<ServiceResponse<IEnumerable<ParkingLaneViewModel>>> GetParkingLaneOfParking(Guid parkingId)
        {
            try
            {
                string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;

                var result = await parkingLaneService.GetParkingLanes(clientId, parkingId);
                return ServiceResponse<IEnumerable<ParkingLaneViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<ParkingLaneViewModel>>.Fail(e);
            }
        }
    }
}

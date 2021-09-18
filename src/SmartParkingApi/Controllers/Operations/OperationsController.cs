using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartParkingAbstract.Services.Operation;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingApi.Controllers.Operations
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService operationService;
        private readonly IHubContext<OperationHub> hubContext;
        private readonly IParkingLaneService parkingLaneService;

        public OperationsController(IOperationService operationService, 
            IHubContext<OperationHub> hubContext,
            IParkingLaneService parkingLaneService)
        {
            this.operationService = operationService;
            this.hubContext = hubContext;
            this.parkingLaneService = parkingLaneService;
        }

        [HttpPost("checkin")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> CheckInWithCard(CheckInParkingRecord cardInfo)
        {
            try
            {
                var record = await operationService.CheckIn(cardInfo);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
            
        }

        [HttpPost("allowin")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> OpenCheckinGate([FromQuery]Guid recordId)
        {
            try
            {
                var record = await operationService.AllowVehicleIn(recordId);
                var parkingLane = await parkingLaneService.GetParkingLaneById(record.CheckinParkingLane.Id);
                foreach (var serialPort in parkingLane.MultiFunctionGates)
                {
                    await hubContext.Clients.Group(serialPort.Name).SendAsync($"OpenGate_{serialPort.Name}");
                }
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }
    }
}

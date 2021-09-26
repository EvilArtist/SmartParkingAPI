using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Operation;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Operation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [HttpPost("allow-enter")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> OpenCheckinGate(AllowParkingViewmodel recordViewModel)
        {
            try
            {
                var recordId = recordViewModel.RecordId;
                var record = await operationService.AllowVehicleEnter(recordId);
                var parkingLane = await parkingLaneService.GetParkingLaneById(record.CheckinParkingLane.Id);
                foreach (var serialPort in parkingLane.MultiFunctionGates)
                {
                    UartDataResponse<string> data = new()
                    {
                        Action = OperationConstants.Action.AllowIn,
                        GateName = serialPort.Name,
                        Data = serialPort.Name
                    };
                   await hubContext.Clients.Group(serialPort.Name).SendAsync($"OpenGate_{serialPort.Name}");
                }
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("discard-checkin")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> DiscardCheckin(AllowParkingViewmodel recordViewModel)
        {
            try
            {
                var recordId = recordViewModel.RecordId;
                var record = await operationService.DeclineVehicleEnter(recordId);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("update-checkin")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> UpdateCheckinData([FromForm]UpdateRecordInfoViewModel data)
        {
            try
            {
                var record = await operationService.UpdateCheckinRecordInfo(data);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("checkout")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> CheckOutWithCard(CheckOutParkingRecord cardInfo)
        {
            try
            {
                var record = await operationService.CheckOut(cardInfo);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("allow-exit")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> OpenCheckoutGate(AllowParkingViewmodel recordViewModel)
        {
            try
            {
                var record = await operationService.AllowVehicleExit(recordViewModel.RecordId);
                var parkingLane = await parkingLaneService.GetParkingLaneById(record.CheckinParkingLane.Id);
                foreach (var serialPort in parkingLane.MultiFunctionGates)
                {
                    UartDataResponse<string> data = new()
                    {
                        Action = OperationConstants.Action.AllowOut,
                        GateName = serialPort.Name,
                        Data = serialPort.Name
                    };
                    await hubContext.Clients.Group(serialPort.Name).SendAsync($"OpenGate_{serialPort.Name}");
                }
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("discard-checkout")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> DiscardCheckout(AllowParkingViewmodel recordViewModel)
        {
            try
            {
                var recordId = recordViewModel.RecordId;
                var record = await operationService.DeclineVehicleExit(recordId);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("update-checkout")]
        public async Task<ServiceResponse<ParkingRecordDetailViewModel>> UpdateCheckoutData([FromForm]UpdateRecordInfoViewModel data)
        {
            try
            {
                var record = await operationService.UpdateCheckoutRecordInfo(data);
                return ServiceResponse<ParkingRecordDetailViewModel>.Success(record);
            }
            catch (Exception e)
            {
                return ServiceResponse<ParkingRecordDetailViewModel>.Fail(e);
            }
        }

        [HttpPost("test")]
        public async Task<ServiceResponse<string>> TestUpload([FromForm] FormData formData)
        {
            var result = await SaveFile(formData.File);
            return ServiceResponse<string>.Success(result);
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var folderName = "Images";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return dbPath;
        }
    }
    public class FormData
    {
        public IFormFile File { get; set; }
    }
}

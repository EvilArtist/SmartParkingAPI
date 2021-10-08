using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Customers;
using SmartParkingAbstract.Services.Data;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.Customers;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDataController : ControllerBase
    {
        private readonly DataParsingResolver dataParsingResolver;
        private readonly IParkingService parkingService;
        private readonly IParkingLaneService parkingLaneService;
        private readonly ICameraConfigService cameraConfigService;
        private readonly ISerialPortService serialPortService;
        private readonly ISlotTypeService slotTypeService;
        private readonly ISlotTypeConfigurationService slotTypeConfigurationService;
        private readonly ICustomerService customerService;

        public ImportDataController(DataParsingResolver dataParsingResolver,
            IParkingService parkingService,
            IParkingLaneService parkingLaneService,
            ICameraConfigService cameraConfigService,
            ISerialPortService serialPortService,
            ISlotTypeService slotTypeService,
            ISlotTypeConfigurationService slotTypeConfigurationService,
            ICustomerService customerService)
        {
            this.dataParsingResolver = dataParsingResolver;
            this.parkingService = parkingService;
            this.parkingLaneService = parkingLaneService;
            this.cameraConfigService = cameraConfigService;
            this.serialPortService = serialPortService;
            this.slotTypeService = slotTypeService;
            this.slotTypeConfigurationService = slotTypeConfigurationService;
            this.customerService = customerService;
        }

        [HttpPost("Import-Parking")]
        public async Task<ServiceResponse<string>> ImportParking(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "Parking"
                };
                var parkingData = service.ParseData<ParkingDataImport>(parsingOption);
                service.Close();
                await parkingService.ImportData(parkingData);
            }
            return ServiceResponse<string>.Success("");
        }


        [HttpPost("Import-ParkingLane")]
        public async Task<ServiceResponse<string>> ImportParkingLane(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "ParkingLane"
                };
                var parkingLaneData = service.ParseData<ParkingLaneDataImport>(parsingOption);
                service.Close();
                await parkingLaneService.ImportData(parkingLaneData);
            }
            return ServiceResponse<string>.Success("");
        }


        [HttpPost("Import-Camera")]
        public async Task<ServiceResponse<IEnumerable<CameraConfigurationViewModel>>> ImportCameraConfig(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "Camera"
                };
                var cameraData = service.ParseData<CameraDataImport>(parsingOption);
                service.Close();
                var result = await cameraConfigService.ImportData(cameraData);
                return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(new List<CameraConfigurationViewModel>() );
        }

        [HttpPost("Import-MultifuncGate")]
        public async Task<ServiceResponse<IEnumerable<SerialPortConfigViewModel>>> ImportMuntifunctionGateConfig(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "SerialPort"
                };
                var devices = service.ParseData<MultigateDataImport>(parsingOption);
                service.Close();
                var result = await serialPortService.ImportData(devices);
                return ServiceResponse<IEnumerable<SerialPortConfigViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<SerialPortConfigViewModel>>.Success(new List<SerialPortConfigViewModel>());
        }

        [HttpPost("Import-SlotType")]
        public async Task<ServiceResponse<IEnumerable<SlotTypeViewModel>>> ImportSlotType(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "ParkingSlot"
                };
                var slotTypes = service.ParseData<SlotTypeDataImport>(parsingOption);
                service.Close();
                var result = await slotTypeService.ImportData(slotTypes);
                return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Success(new List<SlotTypeViewModel>());
        }
            
        [HttpPost("Import-SlotTypeConfig")]
        public async Task<ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>> ImportSlotTypeConfig(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "ParkingSlotConfig"
                };

                Func<string, string, int, SlotTypeConfigDataImport> assignment = 
                    (string parkingName, string slotName, int numberSlot) =>
                    new SlotTypeConfigDataImport()
                    {
                        ParkingName = parkingName,
                        SlotName = slotName,
                        NumberOfSlot = numberSlot
                    };
                var slotTypeConfig = service.ParseData(assignment, parsingOption);
                service.Close();
                var result = await slotTypeConfigurationService.ImportData(slotTypeConfig);
                return ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>.Success(new List<SlotTypeConfigViewModel>());
        }

        [HttpPost("Import-Customer")]
        public async Task<ServiceResponse<IEnumerable<CustomerViewModel>>> ImportCustomers(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "Customer"
                };
                var customers = service.ParseData<CustomerDataImport>(parsingOption);
                service.Close();
                var result = await customerService.ImportData(customers);
                return ServiceResponse<IEnumerable<CustomerViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<CustomerViewModel>>.Success(new List<CustomerViewModel>());
        }
    }
}

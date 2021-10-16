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
        private readonly ISubscriptionTypeService subscriptionTypeService;
        private readonly IVehicleTypeService vehicleTypeService;
        private readonly ICardService cardService;
        private readonly ServiceError unsupportedError = new()
        {
            ErrorCode = "ERR_415",
            ErrorMessage = "Unsupport media type"
        };


        public ImportDataController(DataParsingResolver dataParsingResolver,
            IParkingService parkingService,
            IParkingLaneService parkingLaneService,
            ICameraConfigService cameraConfigService,
            ISerialPortService serialPortService,
            ISlotTypeService slotTypeService,
            ISlotTypeConfigurationService slotTypeConfigurationService,
            ICustomerService customerService,
            ISubscriptionTypeService subscriptionTypeService,
            IVehicleTypeService vehicleTypeService,
            ICardService cardService)
        {
            this.dataParsingResolver = dataParsingResolver;
            this.parkingService = parkingService;
            this.parkingLaneService = parkingLaneService;
            this.cameraConfigService = cameraConfigService;
            this.serialPortService = serialPortService;
            this.slotTypeService = slotTypeService;
            this.slotTypeConfigurationService = slotTypeConfigurationService;
            this.customerService = customerService;
            this.subscriptionTypeService = subscriptionTypeService;
            this.vehicleTypeService = vehicleTypeService;
            this.cardService = cardService;
        }

        [HttpPost("Import-Parking")]
        public async Task<ServiceResponse<IEnumerable<ParkingViewModel>>> ImportParking(IEnumerable<ParkingDataImport> parkingData)
        {
            var result = await parkingService.ImportData(parkingData);
            return ServiceResponse<IEnumerable<ParkingViewModel>>.Success(result);
        }

        [HttpPost("Import-ParkingLane")]
        public async Task<ServiceResponse<IEnumerable<ParkingLaneViewModel>>> ImportParkingLane(IEnumerable<ParkingLaneDataImport> parkingLaneData)
        {
            var result = await parkingLaneService.ImportData(parkingLaneData);
            return ServiceResponse<IEnumerable<ParkingLaneViewModel>>.Success(result);
        }


        [HttpPost("Import-Camera")]
        public async Task<ServiceResponse<IEnumerable<CameraConfigurationViewModel>>> ImportCameraConfig(IEnumerable<CameraDataImport> cameraData)
        {
            var result = await cameraConfigService.ImportData(cameraData);
            return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(result);
        }

        [HttpPost("Import-MultifuncGate")]
        public async Task<ServiceResponse<IEnumerable<SerialPortConfigViewModel>>> ImportMuntifunctionGateConfig(IEnumerable<MultigateDataImport> devices)
        {
            var result = await serialPortService.ImportData(devices);
            return ServiceResponse<IEnumerable<SerialPortConfigViewModel>>.Success(result);
        }

        [HttpPost("Import-SlotType")]
        public async Task<ServiceResponse<IEnumerable<SlotTypeViewModel>>> ImportSlotType(IEnumerable<SlotTypeDataImport> slotTypes)
        {
            var result = await slotTypeService.ImportData(slotTypes);
            return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Success(result);
        }
            
        [HttpPost("Import-SlotTypeConfig")]
        public async Task<ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>> ImportSlotTypeConfig(IEnumerable<SlotTypeConfigDataImport> slotTypeConfig)
        {
            var result = await slotTypeConfigurationService.ImportData(slotTypeConfig);
            return ServiceResponse<IEnumerable<SlotTypeConfigViewModel>>.Success(result);
        }

        [HttpPost("Import-Customer")]
        public async Task<ServiceResponse<IEnumerable<CustomerViewModel>>> ImportCustomers(IEnumerable<CustomerDataImport> customers)
        {
            var result = await customerService.ImportData(customers);
            return ServiceResponse<IEnumerable<CustomerViewModel>>.Success(result);
        }

        [HttpPost("Import-SubscriptionType")]
        public async Task<ServiceResponse<IEnumerable<SubscriptionTypeViewModel>>> ImportSubscriptionType(IEnumerable<SubscriptionTypeDataImport> subscriptionTypes)
        {
            var result = await subscriptionTypeService.ImportData(subscriptionTypes);
            return ServiceResponse<IEnumerable<SubscriptionTypeViewModel>>.Success(result);
        }

        [HttpPost("Import-VehicleType")]
        public async Task<ServiceResponse<IEnumerable<VehicleTypeViewModel>>> ImportVehicleType(IEnumerable<VehicleTypeDataImport> vehicleTypes)
        {
            var result = await vehicleTypeService.ImportData(vehicleTypes);
            return ServiceResponse<IEnumerable<VehicleTypeViewModel>>.Success(result);
        }

        [HttpPost("Import-Card")]
        public async Task<ServiceResponse<IEnumerable<CardViewModel>>> ImportCard(IEnumerable<CardDataImport> cards)
        {
            var result = await cardService.ImportData(cards);
            return ServiceResponse<IEnumerable<CardViewModel>>.Success(result);
        }

        #region Parsing Data
        [HttpPost("Parse-Parking")]
        public ServiceResponse<IEnumerable<ParkingDataImport>> ParseParking(IFormFile file)
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
                return ServiceResponse<IEnumerable<ParkingDataImport>>.Success(parkingData);
            }
            return ServiceResponse<IEnumerable<ParkingDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-ParkingLane")]
        public ServiceResponse<IEnumerable<ParkingLaneDataImport>> ParseParkingLane(IFormFile file)
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
                return ServiceResponse<IEnumerable<ParkingLaneDataImport>>.Success(parkingLaneData);
            }
            return ServiceResponse<IEnumerable<ParkingLaneDataImport>>.Fail(unsupportedError);
        }


        [HttpPost("Parse-Camera")]
        public ServiceResponse<IEnumerable<CameraDataImport>> ParseCameraConfig(IFormFile file)
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
                return ServiceResponse<IEnumerable<CameraDataImport>>.Success(cameraData);
            }
            return ServiceResponse<IEnumerable<CameraDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-MultifuncGate")]
        public ServiceResponse<IEnumerable<MultigateDataImport>> ParseMuntifunctionGateConfig(IFormFile file)
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
                return ServiceResponse<IEnumerable<MultigateDataImport>>.Success(devices);
            }
            return ServiceResponse<IEnumerable<MultigateDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-SlotType")]
        public ServiceResponse<IEnumerable<SlotTypeDataImport>> ParseSlotType(IFormFile file)
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
                return ServiceResponse<IEnumerable<SlotTypeDataImport>>.Success(slotTypes);
            }
            return ServiceResponse<IEnumerable<SlotTypeDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-SlotTypeConfig")]
        public ServiceResponse<IEnumerable<SlotTypeConfigDataImport>> ParseSlotTypeConfig(IFormFile file)
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
                return ServiceResponse<IEnumerable<SlotTypeConfigDataImport>>.Success(slotTypeConfig.OrderBy(x=>x.ParkingName));
            }
            return ServiceResponse<IEnumerable<SlotTypeConfigDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-Customer")]
        public ServiceResponse<IEnumerable<CustomerDataImport>> ParseCustomers(IFormFile file)
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
                return ServiceResponse<IEnumerable<CustomerDataImport>>.Success(customers);
            }
            return ServiceResponse<IEnumerable<CustomerDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-SubscriptionType")]
        public ServiceResponse<IEnumerable<SubscriptionTypeDataImport>> ParseSubscriptionType(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "SubscriptionType"
                };
                var subscriptionTypes = service.ParseData<SubscriptionTypeDataImport>(parsingOption);
                service.Close();
                return ServiceResponse<IEnumerable<SubscriptionTypeDataImport>>.Success(subscriptionTypes);
            }
            return ServiceResponse<IEnumerable<SubscriptionTypeDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-VehicleType")]
        public ServiceResponse<IEnumerable<VehicleTypeDataImport>> ParseVehicleType(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "VehicleType"
                };
                var subscriptionTypes = service.ParseData<VehicleTypeDataImport>(parsingOption);
                service.Close();
                return ServiceResponse<IEnumerable<VehicleTypeDataImport>>.Success(subscriptionTypes);
            }
            return ServiceResponse<IEnumerable<VehicleTypeDataImport>>.Fail(unsupportedError);
        }

        [HttpPost("Parse-Card")]
        public ServiceResponse<IEnumerable<CardDataImport>> ParseCard(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                IDataParsingService service = dataParsingResolver("EXCEL");
                service.Open(stream);
                ExcelParsingOption parsingOption = new()
                {
                    IgnoredIfFailed = true,
                    SheetName = "Card"
                };
                var cards = service.ParseData<CardDataImport>(parsingOption);
                service.Close();
                return ServiceResponse<IEnumerable<CardDataImport>>.Success(cards);
            }
            return ServiceResponse<IEnumerable<CardDataImport>>.Fail(unsupportedError);
        }
        #endregion
    }
}

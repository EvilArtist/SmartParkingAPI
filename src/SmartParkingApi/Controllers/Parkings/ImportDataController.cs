using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Data;
using SmartParkingAbstract.Services.Parking;
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

        public ImportDataController(DataParsingResolver dataParsingResolver,
            IParkingService parkingService,
            IParkingLaneService parkingLaneService,
            ICameraConfigService cameraConfigService
            )
        {
            this.dataParsingResolver = dataParsingResolver;
            this.parkingService = parkingService;
            this.parkingLaneService = parkingLaneService;
            this.cameraConfigService = cameraConfigService;
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
                var cameraData = service.ParseData<CameraImportData>(parsingOption);
                service.Close();
                var result = await cameraConfigService.ImportData(cameraData);
                return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(new List<CameraConfigurationViewModel>() );
        }

        [HttpPost("Import-Multigate")]
        public async Task<ServiceResponse<IEnumerable<CameraConfigurationViewModel>>> ImportMuntifunctionGateConfig(IFormFile file)
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
                var cameraData = service.ParseData<MultigateImportData>(parsingOption);
                service.Close();
                var result = await cameraConfigService.ImportData(cameraData);
                return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(result);
            }
            return ServiceResponse<IEnumerable<CameraConfigurationViewModel>>.Success(new List<CameraConfigurationViewModel>());
        }
    }
}

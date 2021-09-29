using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Data;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
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

        public ImportDataController(DataParsingResolver dataParsingResolver,
            IParkingService parkingService,
            IParkingLaneService parkingLaneService
            )
        {
            this.dataParsingResolver = dataParsingResolver;
            this.parkingService = parkingService;
            this.parkingLaneService = parkingLaneService;
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
    }
}

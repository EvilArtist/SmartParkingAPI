using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.ViewModels.General;
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
        public ImportDataController()
        {

        }

        [HttpPost("Import-Parking")]
        public Task<ServiceResponse<string>> ImportParking(IFormFile file)
        {
            using var stream = file.OpenReadStream();

        }
    }
}

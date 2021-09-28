using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Operation
{
    public class UpdateRecordInfoViewModel
    {
        public Guid RecordId { get; set; }
        public IFormFile FrontCamera { get; set; }
        public IFormFile BackCamera { get; set; }
        public string LicensePlate{ get; set; }
    }
}

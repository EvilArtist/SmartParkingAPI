using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class CustomerDataImport
    {
        [ExcelDataImport("Mã khách hàng")]
        [Required]
        public string CustomerCode { get; set; }
        [ExcelDataImport("Họ")]
        [Required]
        public string FirstName { get; set; }
        [ExcelDataImport("Tên")]
        [Required]
        public string LastName { get; set; }
        [ExcelDataImport("Loại khách hàng")]
        [Required]
        public string CustomerTypeCode { get; set; }
    }
}

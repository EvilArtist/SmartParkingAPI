using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class CardDataImport
    {
        [ExcelDataImport("Tên thẻ")]
        [Required]
        public string Name { get; set; }
        [ExcelDataImport("Mã thẻ")]
        [Required]
        public string IdentityCode { get; set; }
        [ExcelDataImport("Loại xe")]
        public string VehicleTypeName { get; set; }
        [ExcelDataImport("Mã khách hàng")]
        public string CustomerName { get; set; }
        [ExcelDataImport("Loại vé")]
        public string SubscriptionTypeName { get; set; }
    }
}

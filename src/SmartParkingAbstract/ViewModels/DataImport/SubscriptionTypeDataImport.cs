using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class SubscriptionTypeDataImport
    {
        [ExcelDataImport("Tên loại thuê bao")]
        [Required]
        public string Name { get; set; }
        [ExcelDataImport("Mô tả")]
        public string Description { get; set; }
    }
}

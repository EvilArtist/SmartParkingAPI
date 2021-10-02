using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class SlotTypeDataImport
    {
        [ExcelDataImport("Tên")]
        [Required]
        public string SlotName { get; set; }
        [ExcelDataImport("Mô tả")]
        public string Description { get; set; }
    }
}

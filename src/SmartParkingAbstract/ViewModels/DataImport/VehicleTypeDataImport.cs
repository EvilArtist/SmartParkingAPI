using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class VehicleTypeDataImport
    {
        [ExcelDataImport("Tên")]
        [Required]
        public string Name { get; set; }
        [ExcelDataImport("Mô tả")]
        [Required]
        public string Description { get; set; }
        [ExcelDataImport("Loại chỗ đậu xe")]
        [Required]
        public string ParkingType { get; set; }
    }
}

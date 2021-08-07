using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class MultigateImportData
    {
        [ExcelDataImport("Tên")]
        [Required]
        public string Name { get; set; }
        [ExcelDataImport("Mã thiết bị")]
        [Required]
        public string DeviceName { get; set; }
        [ExcelDataImport("Tốc độ")]
        public string Baudrate { get; set; } = "9600";
        [ExcelDataImport("Nhà sản xuất")]
        public string Oem { get; set; }
    }
}

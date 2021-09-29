using SmartParking.Share.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class ParkingDataImport
    {
        [ExcelDataImport("Tên bãi xe")]
        [Required]
        public string Name { get; set; }

        [ExcelDataImport("Địa chỉ")]
        public string Address { get; set; }
    }
}

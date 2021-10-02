using SmartParking.Share.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class ParkingLaneDataImport
    {
        [ExcelDataImport("Tên bãi xe")]
        public string ParkingName { get; set; }

        [ExcelDataImport("Tên làn xe")]
        [Required]
        public string Name { get; set; }

        [ExcelDataImport("Tên camera")]
        public string Cameras { get; set; }

        [ExcelDataImport("Tên cổng đa năng")]
        public string MultiFunctionGates { get; set; }
    }
}

using SmartParking.Share.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class CameraDataImport
    {
        [ExcelDataImport("Tên")]
        [Required]
        public string CameraName { get; set; }
        [ExcelDataImport("Tên server")]
        [Required]
        public string ServerName { get; set; }
        [ExcelDataImport("Tên đăng nhập")]
        [Required]
        public string UserName { get; set; }
        [ExcelDataImport("Mật khẩu")]
        [Required]
        public string Password { get; set; }
        [ExcelDataImport("URL Template")]
        [Required]
        public string URLTemplate { get; set; }
        [ExcelDataImport("Camera Id")]
        public int CameraId { get; set; }
        [ExcelDataImport("Stream Id")]
        public int StreamId { get; set; }
        [ExcelDataImport("Nhà sản xuất")]
        public string Oem { get; set; }
        [ExcelDataImport("Giao thức")]
        public string Protocol { get; set; }
    }
}

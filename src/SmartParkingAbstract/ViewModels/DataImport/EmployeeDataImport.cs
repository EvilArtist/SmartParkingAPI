using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public class EmployeeDataImport
    {
        [ExcelDataImport("Mã nhân viên")]
        [Required]
        public string UserName { get; set; }
        [ExcelDataImport("Email")]
        [Required]
        public string Email { get; set; }
        [ExcelDataImport("Số điện thoại")]
        public string Phone { get; set; }
        [ExcelDataImport("Tên")]
        public string FirstName { get; set; }
        [ExcelDataImport("Họ")]
        public string LastName { get; set; }
        [ExcelDataImport("Địa chỉ")]
        public string Address { get; set; }
        [ExcelDataImport("Số CCCD")]
        public string IDCardNumber { get; set; }
        [ExcelDataImport("Chức danh")]
        public string Role { get; set; }
    }
}

using SmartParkingAbstract.ViewModels.General;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class EmployeeCreateModel : MutiTanentModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        public string IDCardNumber { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}

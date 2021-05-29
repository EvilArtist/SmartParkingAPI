using SmartParkingAbstract.ViewModels.General;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class EmployeeCreateModel : MutiTanentModel
    {
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        [Required]
        public virtual string FirstName { get; set; }
        [Required]
        public virtual string LastName { get; set; }
        public virtual string Address { get; set; }
        [Required]
        public virtual string IDCardNumber { get; set; }
        [Required]
        public virtual Guid RoleId { get; set; }
    }
}

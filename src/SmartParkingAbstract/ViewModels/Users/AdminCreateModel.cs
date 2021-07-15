using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class AdminCreateModel : MultiTanentModel
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
    }
}

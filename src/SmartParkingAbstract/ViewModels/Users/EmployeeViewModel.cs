using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class EmployeeViewModel 
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
    }
}

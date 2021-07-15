using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class RolePolicyAssignmentViewModel: MultiTanentModel
    {
        public string Role { get; set; }
        public string Policy { get; set; }
    }
}

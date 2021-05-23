using System;
using System.Collections.Generic;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class PoliciesViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<string> Policies{ get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class EmployeeDetail
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string IDCardNumber { get; set; }
        public Guid RoleId { get; set; }
    }
}

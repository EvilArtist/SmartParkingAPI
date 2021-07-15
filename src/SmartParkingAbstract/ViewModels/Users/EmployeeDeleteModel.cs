using SmartParkingAbstract.ViewModels.General;
using System;

namespace SmartParkingAbstract.ViewModels.Users
{
    public class EmployeeDeleteModel: MultiTanentModel
    {
        public Guid UserId { get; set; }
    }
}

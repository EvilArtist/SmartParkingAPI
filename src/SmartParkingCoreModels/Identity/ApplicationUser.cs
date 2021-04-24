using Microsoft.AspNetCore.Identity;
using SmartParking.Share;
using System;
using System.Collections.Generic;

namespace SmartParkingCoreModels.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public UserStatus Status { get; set; }
        public string ClientId { get; set; }
        public string ParkingId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string IDCardNumber { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}

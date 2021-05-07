using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Identity
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public string ClientId { get; set; }
        public bool IsCustomerRole { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
        public const string Security = "Security";
        public const string Accounter = "Accounter";
        public const string SecurityLeader = "SecurityLeader";
        public const string Manager = "Manager";
        public static readonly string[] DefaultRoles =
        {
            Admin,
            SuperAdmin,
            Security,
            SecurityLeader,
            Accounter,
            Manager
        };
    }
}

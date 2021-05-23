using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Quản trị viên";
        public const string Security = "Bảo vệ";
        public const string Accounter = "Kế toán";
        public const string SecurityLeader = "Trưởng nhóm bảo vệ";
        public const string Manager = "Giám đốc";
        public static readonly string[] DefaultRoles =
        {
            SuperAdmin,
            Admin,
            Security,
            SecurityLeader,
            Accounter,
            Manager
        };
    }
}

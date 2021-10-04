using SmartParking.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class CustomerConstants
    {
        public static NamedObjectModel<string> CustomerTypeBussiness = new()
        {
            Name = "Doanh nghiệp",
            Code = "BUSSINESS",
            Description = "Doanh nghiệp"
        };
        public static NamedObjectModel<string> CustomerTypePersonal = new()
        {
            Name = "Cá nhân",
            Code = "PERSONAL",
            Description = "Vãng lai"
        };
        public static List<NamedObjectModel<string>> DefaultCustomerTypes => new()
        {
            CustomerTypeBussiness,
            CustomerTypePersonal
        };

    }
}

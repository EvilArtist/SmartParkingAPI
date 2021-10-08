using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Customers
{
    public class Customer : AuditModel
    {
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string CustomerCode { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string FirstName { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string LastName { get; set; }
        [ForeignKey(nameof(CustomerType))]
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string CustomerTypeCode { get; set; }
        public CustomerType CustomerType { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}

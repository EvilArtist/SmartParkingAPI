using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking.PriceBooks
{
    public class PriceBook : AuditModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        [ForeignKey(nameof(Condition))]
        public Guid PriceBookConditionId { get; set; }
        public PriceBookCondition Condition { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<PriceList> PriceLists { get; set; }
    }
}

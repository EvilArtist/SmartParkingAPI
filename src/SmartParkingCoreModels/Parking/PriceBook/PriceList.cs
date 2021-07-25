using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking.PriceBook
{
    public class PriceList : MultiTanentModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        [ForeignKey(nameof(Condition))]
        public Guid PriceListConditionId { get; set; }
        public PriceListCondition Condition { get; set; }
        public PriceCalculation Calculation { get; set; }
    }
}

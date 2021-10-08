using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Parking;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParkingCoreModels.Customers
{
    public class Subscription: AuditModel
    {
        public DateTime? LastExtendDate { get; set; }
        public DateTime? NextExtendDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        [Column(TypeName = "decimal(9, 0)")]
        public decimal Amount { get; set; }
        [ForeignKey(nameof(Vehicle))]
        public Guid? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [ForeignKey(nameof(AssignedCard))]
        public Guid CardId { get; set; }
        public Card AssignedCard { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }

}

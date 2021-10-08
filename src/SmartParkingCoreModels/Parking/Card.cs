using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class Card : AuditModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string IdentityCode { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        [ForeignKey(nameof(Status))]
        public string StatusCode { get; set; }
        public CardStatus Status { get; set; }
        public virtual ICollection<CardParkingAssignment> ParkingAssignments { get; set; }
        public Guid? SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}

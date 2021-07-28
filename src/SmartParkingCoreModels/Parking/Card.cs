using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class Card : AuditModel
    {
        public string Name { get; set; }
        public string IdentityCode { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        [ForeignKey(nameof(Status))]
        public Guid CardStatusId { get; set; }
        public CardStatus Status { get; set; }
        public virtual ICollection<CardParkingAssignment> ParkingAssignments { get; set; }
        // public string CustomerName { get; set; }
    }
}

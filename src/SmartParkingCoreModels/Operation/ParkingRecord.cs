using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Identity;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Operation
{
    public class ParkingRecord : AuditModel
    {
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string LaneName { get; set; }
        public Guid ParkingLaneId { get; set; }
        public ParkingLane ParkingLane { get; set; }
        public Guid CheckinEmployeeId { get; set; }
        public ApplicationUser CheckinEmployee { get; set; }
        public Guid? CheckoutEmployeeId { get; set; }
        public ApplicationUser CheckoutEmployee { get; set; }
        public string URLCheckinFrontImage { get; set; }
        public string URLCheckinBackImage { get; set; }
        public string URLCheckoutFrontImage { get; set; }
        public string URLCheckoutBackImage { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string CheckinPlateNumber { get; set; }
        public string CheckoutPlateNumber { get; set; }
    }
}

using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Identity;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Guid ParkingId { get; set; }
        public Guid CheckinParkingLaneId { get; set; }
        public ParkingLane CheckinParkingLane { get; set; }
        public Guid? CheckoutParkingLaneId { get; set; }
        public ParkingLane CheckoutParkingLane { get; set; }
        public Guid CheckinEmployeeId { get; set; }
        public Guid? CheckoutEmployeeId { get; set; }
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
        
        [ForeignKey(nameof(Status))]
        public string StatusCode { get; set; }
        public ParkingRecordStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ICollection<PriceItem> PriceItems { get; set; }
    }
}

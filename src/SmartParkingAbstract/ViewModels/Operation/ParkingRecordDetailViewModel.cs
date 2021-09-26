using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Operation
{
    public class ParkingRecordDetailViewModel
    {
        public Guid Id { get; set; }
        public CardViewModel Card { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string LaneName { get; set; }
        public ParkingLaneViewModel CheckinParkingLane { get; set; }
        public ParkingLaneViewModel CheckoutParkingLane { get; set; }
        public Guid CheckinEmployeeId { get; set; }
        public Guid? CheckoutEmployeeId { get; set; }
        public string URLCheckinFrontImage { get; set; }
        public string URLCheckinBackImage { get; set; }
        public string URLCheckoutFrontImage { get; set; }
        public string URLCheckoutBackImage { get; set; }
        public VehicleTypeViewModel VehicleType { get; set; }
        public SubscriptionTypeViewModel SubscriptionType { get; set; }
        public string CheckinPlateNumber { get; set; }
        public string CheckoutPlateNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusCode { get; set; }
        public ParkingRecordStatusViewModel Status { get; set; }
        public List<PriceItemViewModel> PriceItems { get; set; }
    }
}

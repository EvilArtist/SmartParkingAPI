using System;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBook
{
    public class PriceCalculationParam
    {
        public string ClientId { get; set; }
        public Guid VehicleTypeId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
    }
}

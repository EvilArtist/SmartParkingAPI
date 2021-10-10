using SmartParkingAbstract.ViewModels.Parking;
using System;

namespace SmartParkingAbstract.ViewModels.Customers
{
    public class VehicleViewModel
    {
        public string LicensePlate { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleTypeViewModel VehicleType { get; set; }
        public Guid? SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }
    }
}
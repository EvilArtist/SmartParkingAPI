using SmartParkingAbstract.ViewModels.General;
using System;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBooks
{
    public class PriceListQuery: QueryModel
    {
        public Guid VehicleTypeId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
    }
}

using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using SmartParkingCoreModels.Parking;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingCoreModels.Customers
{
    public class Vehicle: AuditModel
    {
        [MaxLength(EntityConstants.LicensePlateMaxLength)]
        public string LicensePlate { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Guid? SubscriptionId { get; set; } 
        public Subscription Subscription { get; set; }
    }

}

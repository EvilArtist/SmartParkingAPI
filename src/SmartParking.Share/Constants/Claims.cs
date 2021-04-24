using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class RoleClaims
    {
        public const string EmployeeView = "EmployeeView";
        public const string EmployeeManager = "EmployeeManager";
        public const string RoleManager = "RoleManager";
        public const string CardView = "CardView";
        public const string CardManager = "CardManager";
        public const string VehicleView = "VehicleView";
        public const string VehicleManager = "VehicleManager";
        public const string SubscriptionView = "SubscriptionView";
        public const string SubscriptionManager = "SubscriptionManager";
        public const string PriceView = "PriceView";
        public const string PriceManager = "PriceManager";
        public const string ParkingView = "ParkingView";
        public const string ParkingManager = "ParkingManager";
        public const string DeviceView = "DeviceView";
        public const string DeviceManager = "DeviceManager";
        public const string Operation = "Operation";
        public const string RecordEdit = "RecordEdit";
        public const string RecordHistoryView = "RecordHistoryView";

        public static List<string> GetClaims() => new()
        {
            EmployeeView,
            EmployeeManager,
            RoleManager,
            CardView,
            CardManager,
            VehicleView,
            VehicleManager,
            SubscriptionView,
            SubscriptionManager,
            PriceView,
            PriceManager,
            ParkingView,
            ParkingManager,
            DeviceView,
            DeviceManager,
            Operation,
            RecordEdit,
            RecordHistoryView
        };

    }
}

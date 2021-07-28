using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class CardConstants
    {

    }
    public class CardStatusCode
    {
        public const string Active = "ACTIVE";
        public const string Parking = "PARKING";
        public const string Checkout = "CHECKOUT";
        public const string Lock = "LOCK";
    }

    public class CardRefinements
    {
        public const string ParkingId = "parkingId";
        public const string NotAssignedToParkingId = "nParkingId";
        public const string CardStatus = "status";
    }
   
}

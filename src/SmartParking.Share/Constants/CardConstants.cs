using SmartParking.Share.Models;
using System;
using System.Collections.Generic;
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
    public class SystemCardStatus
    {
        public static List<NamedObjectModel<string>> Defaults = new()
        {
            new()
            {
                Name = "Hoạt động",
                Code = CardStatusCode.Active,
                Description = "Thẻ mới tạo, hoặc thẻ vãng lai",
            },
            new()
            {
                Name = "Trong bãi xe",
                Code = CardStatusCode.Parking,
                Description = "Khi có khách đang sử dụng",
            },
            new()
            {
                Name = "Ngoài bãi xe",
                Code = CardStatusCode.Checkout,
                Description = "Thẻ khách hàng, đã được mang ra ngoài",
            },
            new()
            {
                Name = "Khoá",
                Code = CardStatusCode.Lock,
                Description = "Thẻ báo mất, hoặc mất xe",
            }
        };
    }

    public class CardRefinements
    {
        public const string ParkingId = "parkingId";
        public const string NotAssignedToParkingId = "nParkingId";
        public const string CardStatus = "status";
    }

    public class SystemSubscriptionType
    {
        public static NamedObjectModel<Guid> DefaultSubscriptionType = new()
        {
            Code = Guid.Empty,
            Name = "Vãng lai",
            Description = "Vãng lai"
        };
        public static NamedObjectModel<Guid>[] GetAll() => new NamedObjectModel<Guid>[]
        {
            DefaultSubscriptionType
        };
    }
}

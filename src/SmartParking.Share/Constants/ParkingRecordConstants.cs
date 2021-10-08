using SmartParking.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Constants
{
    public class ParkingRecordConstants
    {
        public static List<NamedObjectModel<string>> SystemStatuses = new()
        {
            new()
            {
                Code = ParkingRecordStatusConstants.Created,
                Name = "Khởi tạo",
                Description = "Bản ghi mới tạo trên hệ thống"
            },

            new()
            {
                Code = ParkingRecordStatusConstants.Checkout,
                Name = "Quẹt thẻ ra",
                Description = "Trạng thái sau khi quẹt thẻ ra"
            },
            new()
            {
                Code = ParkingRecordStatusConstants.Complete,
                Name = "Hoàn thành",
                Description = "Trạng thái hoàn tất, sau khi xe đã ra khỏi bãi"
            },
            new()
            {
                Code = ParkingRecordStatusConstants.LostTicket,
                Name = "Mất thẻ xe",
                Description = "Trạng thái khi mất thẻ xe"
            },
            new()
            {
                Code = ParkingRecordStatusConstants.LostVehicle,
                Name = "Mất thẻ xe",
                Description = "Trạng thái khi mất xe"
            },
            new()
            {
                Code = ParkingRecordStatusConstants.Parking,
                Name = "Xe trong bãi",
                Description = "Trạng thái khi vào trong bãi"
            },
            new()
            {
                Code = ParkingRecordStatusConstants.Checkin,
                Name = "Quẹt thẻ vào",
                Description = "Trạng thái sau khi quẹt thẻ vào bãi, chờ xử lý vào bãi"
            }
        };
    }

    public class ParkingRecordStatusConstants
    {
        public const string Parking = "PARKING";
        public const string Checkin = "CHECKIN";
        public const string Checkout = "CHECKOUT";
        public const string Complete = "COMPLETE";
        public const string LostTicket = "LOST_TICKET";
        public const string LostVehicle = "LOST_VEHICLE";
        public const string Created = "CREATED";
        public const string Declined = "DECLINED";
    }
}

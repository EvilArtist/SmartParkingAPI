using SmartParking.Share.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingCoreModels.Operation
{
    public class PriceItem
    {
        public Guid Id { get; set; }
        public PriceFormular Type { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        public double Price { get; set; }
        public double HourBlock { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid ParkingRecordId { get; set; }
        public ParkingRecord ParkingRecord { get; set; }
    }
}

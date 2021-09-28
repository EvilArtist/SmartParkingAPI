using SmartParking.Share.Constants;
using System;

namespace SmartParkingAbstract.ViewModels.Operation
{
    public class PriceItemViewModel
    {
        public Guid Id { get; set; }
        public PriceFormular Type { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double HourBlock { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid ParkingRecordId { get; set; }
    }
}

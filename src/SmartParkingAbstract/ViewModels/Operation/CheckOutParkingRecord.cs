using System;

namespace SmartParkingAbstract.ViewModels.Operation
{
    public class CheckOutParkingRecord
    {
        public string CardCode { get; set; }
        public Guid ParkingId { get; set; }
        public Guid ParkingLaneId { get; set; }
    }
}

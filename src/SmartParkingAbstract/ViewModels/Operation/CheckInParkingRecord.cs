using System;

namespace SmartParkingAbstract.ViewModels.Operation
{
    public class CheckInParkingRecord
    {
        public string CardCode { get; set; }
        public Guid ParkingId { get; set; }
        public Guid ParkingLaneId { get; set; }
    }
}

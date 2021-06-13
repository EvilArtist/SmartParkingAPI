using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class SerialPortConfigViewModel : MutiTanentModel
    {
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public string Baudrate { get; set; }
        public string Oem { get; set; }
        public string Status { get; set; }
        public Guid ParkingId { get; set; }
        public ParkingViewModel Parking { get; set; }
    }
}

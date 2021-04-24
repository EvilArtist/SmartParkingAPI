using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class SerialPortConfiguration : AuditModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public string Baudrate { get; set; }
        public string Oem { get; set; }
        public DeviceStatus Status { get; set; }
    }
}

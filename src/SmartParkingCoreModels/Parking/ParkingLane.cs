using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class ParkingLane: AuditModel
    {
        public string Name { get; set; }
        public ICollection<CameraConfiguration> Cameras { get; set; }
        public ICollection<SerialPortConfiguration> MutiFunctionGates { get; set; }
        public Guid? ParkingId { get; set; }
        public ParkingConfig Parking { get; set; }
    }
}

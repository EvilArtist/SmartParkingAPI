using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class ParkingLaneViewModel : MultiTanentModel
    {
        public Guid ParkingId { get; set; }
        public string Name { get; set; }
        public ICollection<CameraConfigurationViewModel> Cameras { get; set; } = new List<CameraConfigurationViewModel>();
        public ICollection<SerialPortConfigViewModel> MultiFunctionGates { get; set; } = new List<SerialPortConfigViewModel>();
    }

    public class CreateUpdateParkingLaneViewModel : MultiTanentModel
    {
        public Guid ParkingId { get; set; }
        public string Name { get; set; }
        public List<Guid> CameraIds { get; set; } = new List<Guid>();
        public List<Guid> MultiFunctionGateIds { get; set; } = new List<Guid>();

    }
}

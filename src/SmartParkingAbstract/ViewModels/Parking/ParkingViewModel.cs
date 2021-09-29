using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class ParkingViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfLanes { get; set; }
        public int NumberOfLots { get; set; }
    }

    public class CreateUpdateParkingViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class ParkingDetailsModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ParkingLaneViewModel> ParkingLanes { get; set; }
        public List<SlotTypeConfigViewModel> SlotTypeConfigs { get; set; }
    }
}

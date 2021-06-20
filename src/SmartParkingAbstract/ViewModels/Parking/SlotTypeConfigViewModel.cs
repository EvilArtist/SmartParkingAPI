using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class SlotTypeConfigViewModel: MutiTanentModel
    {
        public Guid ParkingId { get; set; }
        public ParkingViewModel Parking { get; set; }
        public SlotTypeViewModel SlotType { get; set; }
        public Guid SlotTypeId { get; set; }
        public int SlotCount { get; set; }
    }
    public class UpdateSlotTypeConfigsViewModel : MutiTanentModel
    {
        public Guid ParkingId { get; set; }
        public IEnumerable<UpdateSlotTypeConfigViewModel> SlotTypeConfigs { get; set; }
    }

    public class UpdateSlotTypeConfigViewModel
    {
        public Guid SlotTypeId { get; set; }
        public int SlotCount { get; set; }
    }
}

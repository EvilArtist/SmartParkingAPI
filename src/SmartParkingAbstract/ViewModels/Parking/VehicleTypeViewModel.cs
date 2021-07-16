using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class VehicleTypeViewModel: MultiTanentModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SlotTypeId { get; set; }
        public string SlotTypeName { get; set; }
        public int CardCount { get; set; }
    }

    public class CreateUpdateVehicleTypeViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SlotTypeId { get; set; }
    }
}

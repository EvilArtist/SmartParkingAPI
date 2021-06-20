using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class ParkingConfig : AuditModel
    {
        public virtual ICollection<SlotTypeConfiguration> SlotTypeConfigurations { get;set;}
    }
}

using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class ParkingConfig : AuditModel
    {
        public virtual ICollection<SlotTypeConfiguration> SlotTypeConfigurations { get;set;}
        public virtual ICollection<ParkingLane> ParkingLanes{ get;set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
    }
}

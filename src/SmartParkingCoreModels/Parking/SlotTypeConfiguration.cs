using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class SlotTypeConfiguration
    {
        public virtual string ClientId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid ParkingId { get; set; }
        public ParkingConfig Parking { get; set; }
        public SlotType SlotType { get; set; }
        public Guid SlotTypeId { get; set; }
        public int SlotCount { get; set; }
    }
}

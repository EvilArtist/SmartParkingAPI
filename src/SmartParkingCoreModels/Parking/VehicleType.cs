using SmartParking.Share.Constants;
using SmartParkingCoreModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Parking
{
    public class VehicleType : AuditModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }
        public Guid SlotTypeId { get; set; }
        public SlotType SlotType { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}

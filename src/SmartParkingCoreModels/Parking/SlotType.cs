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
    /// <summary>
    /// Loại chỗ đậu xe
    /// </summary>
    public class SlotType : AuditModel
    {
        [MaxLength(EntityConstants.NameMaxLength)]
        public string SlotName { get; set; }
        [MaxLength(EntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public virtual ICollection<SlotTypeConfiguration> SlotTypeConfigurations { get; set; }
    }
}

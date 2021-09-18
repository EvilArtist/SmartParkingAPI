using SmartParking.Share.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Operation
{
    public class ParkingRecordStatus
    {
        [Key]
        [MaxLength(EntityConstants.CodeMaxLength)]
        public string Code { get; set; }
        [MaxLength(EntityConstants.NameMaxLength)]
        public string Name { get; set; }
        [MaxLength(EntityConstants.ShortDescriptionMaxLength)]
        public string Description { get; set; }
    }
}

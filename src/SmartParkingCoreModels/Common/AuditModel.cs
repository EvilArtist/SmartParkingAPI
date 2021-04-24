using SmartParkingCoreModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Common
{
    public abstract class AuditModel
    {
        public  DateTime? CreateTime { get; set; }
        public  DateTime? UpdateTime { get; set; }

        [ForeignKey("UpdatedBy")]
        public  Guid? UpdatedById { get; set; }
        public ApplicationUser UpdatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public  Guid? CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}

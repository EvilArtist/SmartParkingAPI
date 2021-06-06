using SmartParkingCoreModels.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreModels.Common
{
    public abstract class AuditModel : MultiTanentModel
    {
        public  DateTime? CreateTime { get; set; }
        public  DateTime? UpdateTime { get; set; }
        public  Guid? UpdatedById { get; set; }
        public  Guid? CreatedById { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public abstract class MutiTanentModel
    {
        [Required]
        public string ClientId { get; set; }
    }
}

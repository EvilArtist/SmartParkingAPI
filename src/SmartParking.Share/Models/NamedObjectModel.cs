using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Models
{
    public class NamedObjectModel<T>
    {
        public T Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

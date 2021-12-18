using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class ValidationViewModel<T>
    {
        public T Field { get; set; }
        public ServiceError Error { get; set; }
    }
}

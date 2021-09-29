using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.DataImport
{
    public abstract class ParsingOption
    {
        public bool IgnoredIfFailed { get; set; }
    }
}

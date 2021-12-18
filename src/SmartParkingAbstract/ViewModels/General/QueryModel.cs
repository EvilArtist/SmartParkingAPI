using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class QueryModel
    {
        public string QueryString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Refinement[] Refinements { get; set; }
    }

    public class Refinement
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}

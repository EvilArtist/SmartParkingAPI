using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class ApiTime
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Hours, Minutes, 0);
        }

        public static ApiTime FromTimeSpan(TimeSpan t)
        {
            return new()
            {
                Hours = t.Hours,
                Minutes = t.Minutes
            };
        }
    }
}

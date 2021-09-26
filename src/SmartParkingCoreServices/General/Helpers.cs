using SmartParkingAbstract.Services.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.General
{
    public class Helpers : IHelpers
    {
        public string GenerateFileName(string prefix = "", string subfix = "")
        {
            string fileName = prefix + DateTime.Now.ToString($"ddMMyyyyTHHmmssfff") + subfix;
            return fileName.Trim();
        }

        public string GetEnumDescription(Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return @enum.ToString();
            }
        }
    }
}

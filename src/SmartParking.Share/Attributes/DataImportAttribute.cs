using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class ExcelDataImportAttribute : Attribute
    {
        public string ColumnHeader{ get; }
        public ExcelDataImportAttribute(string columnHeader)
        {
            ColumnHeader = columnHeader;
        }
    }
}

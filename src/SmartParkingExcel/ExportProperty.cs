using SmartParking.Share.Attributes;
using System;
using System.Collections.Generic;

namespace SmartParkingExcel
{
    public class ExportProperty
    {
        public string PropertyName { get; set; }
        public string ColumnHeader { get; set; }
        public string ColumnName { get; set; }

        public static List<ExportProperty> OfType(Type type)
        {
            var properties = type.GetProperties();
            var exportProperties = new List<ExportProperty>();
            foreach (var property in properties)
            {
                if (Attribute.GetCustomAttribute(property, typeof(ExcelDataImportAttribute)) is ExcelDataImportAttribute dataAttributes)
                {
                    exportProperties.Add(new ExportProperty()
                    {
                        ColumnHeader = dataAttributes.ColumnHeader,
                        PropertyName = property.Name,
                        ColumnName = ""
                    });
                }
            }
            return exportProperties;
        }
    }
}


using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SmartParking.Share.Attributes;
using SmartParkingAbstract.Services.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartParkingExcel
{
    public class ExcelDataParsingService : IDataParsingService
    {
        private SpreadsheetDocument document;
        public void Open(Stream stream)
        {
            document = SpreadsheetDocument.Open(stream, false);
        }

        public IEnumerable<T> ParseData<T>(string sheetName, bool ignoreWhenFail) where T: new()
        {
            if(document == null)
            {
                return new List<T>();
            }
            var workbook = document.WorkbookPart.Workbook;
            var sheet = workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == sheetName);
            if (sheet == null)
            {
                return new List<T>();
            }
            WorksheetPart wsPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheet.Id));

            var dataHeaders = ExportProperty.OfType(typeof (T));
            Dictionary<string, string> headerColumns = new();
            for (int i = 0; i < dataHeaders.Count + 1; i++)
            {
                char column = (char)('A' + i);
                var value = ReadCellValue(document.WorkbookPart, wsPart, column + "" + 1);
                var header = dataHeaders.FirstOrDefault(x => x.ColumnHeader == value);
                if (header != null)
                {
                    header.ColumnName = column.ToString();
                }
            }
            if (dataHeaders.Any(x=> string.IsNullOrEmpty(x.ColumnName)))
            {
                throw new InvalidCastException();
            }

            int row = 2;
            var firstColumn = ReadCellValue(document.WorkbookPart, wsPart, "A" + row);
            List<T> importData = new();
            while (!string.IsNullOrEmpty(firstColumn))
            {
                try
                {
                    T employeeData = ReadRowData<T>(dataHeaders, workbook.WorkbookPart, wsPart, row);
                    importData.Add(employeeData);
                }
                catch (Exception )
                {
                    if(!ignoreWhenFail)
                    {
                        throw;
                    }
                }
                row++;
                firstColumn = ReadCellValue(document.WorkbookPart, wsPart, "A" + row);
            }
            return importData;
        }

        public void Close()
        {
            document.Close();
        }

        private static T ReadRowData<T>(List<ExportProperty> properties, WorkbookPart wbPart, WorksheetPart wsPart, int row) where T:new()
        {
            T data = new();
            Type type = typeof(T);
            foreach (var property in properties)
            {
                var assamblyProperty = type.GetProperty(property.PropertyName);
                if (assamblyProperty != null)
                {
                    string value = ReadCellValue(wbPart, wsPart, property.ColumnName + row);
                    assamblyProperty.SetValue(data, Convert.ChangeType(value, assamblyProperty.PropertyType));
                }
            }
            return data;
        }

        private static string ReadCellValue(WorkbookPart wbPart, WorksheetPart wsPart, string addressName)
        {
            Cell cell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == addressName).FirstOrDefault();
            if (cell == null)
            {
                return "";
            }
            string cellValue = "";
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var stringId = Convert.ToInt32(cell.InnerText);
                cellValue = wbPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(stringId).InnerText;
            }
            else
            {
                cellValue = cell.InnerText;
            }
            return cellValue;
        }
    }

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


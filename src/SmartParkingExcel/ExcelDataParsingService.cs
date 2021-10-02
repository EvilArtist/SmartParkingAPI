using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SmartParkingAbstract.Services.Data;
using SmartParkingAbstract.ViewModels.DataImport;
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

        public IEnumerable<T> ParseData<T>(ParsingOption parsingOption) where T: new()
        {
            if (parsingOption is not ExcelParsingOption excelParsingOption || string.IsNullOrEmpty(excelParsingOption.SheetName))
            {
                return new List<T>();
            }
            if (document == null)
            {
                return new List<T>();
            }
            var workbook = document.WorkbookPart.Workbook;
            var sheet = workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == excelParsingOption.SheetName);
            if (sheet == null)
            {
                return new List<T>();
            }
            WorksheetPart wsPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

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
                    T data = ReadRowData<T>(dataHeaders, workbook.WorkbookPart, wsPart, row);
                    importData.Add(data);
                }
                catch (Exception )
                {
                    if(!excelParsingOption.IgnoredIfFailed)
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
            document = null;
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

        public IEnumerable<T> ParseData<T, T1>( 
            Func<string, string, T1, T> assignment, 
            ParsingOption parsingOption) where T : new()
        {
            if (parsingOption is not ExcelParsingOption excelParsingOption || string.IsNullOrEmpty(excelParsingOption.SheetName))
            {
                return new List<T>();
            }
            if (document == null)
            {
                return new List<T>();
            }
            var workbook = document.WorkbookPart.Workbook;
            var sheet = workbook.Descendants<Sheet>().FirstOrDefault(x => x.Name == excelParsingOption.SheetName);
            if (sheet == null)
            {
                return new List<T>();
            }
            WorksheetPart wsPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheet.Id));

            Dictionary<string, string> headerColumns = new();
            int column = 2;
            var headerValue = ReadCellValue(document.WorkbookPart, wsPart, GetExcelColumnName(column) + "" + 1);
            var headers = new List<string>();
            while (!string.IsNullOrEmpty(headerValue))
            {
                headers.Add(headerValue);
                column++;
                headerValue = ReadCellValue(document.WorkbookPart, wsPart, GetExcelColumnName(column) + 1);
            }

            var rows = new List<string>();
            int row = 2;
            var rowHeader = ReadCellValue(document.WorkbookPart, wsPart, "A" + row);
            var result = new List<T>();
            while (!string.IsNullOrEmpty(rowHeader))
            {
                for (column = 2; column < headers.Count + 2; column++)
                {
                    var cellValue = ReadCellValue(document.WorkbookPart, wsPart, GetExcelColumnName(column) + row);
                    var value = (T1)Convert.ChangeType(cellValue, typeof(T1));
                    var data = assignment(headers[column - 2], rowHeader, value);
                    result.Add(data);
                }
                row++;
                rowHeader = ReadCellValue(document.WorkbookPart, wsPart, "A" + row);
            }
            return result;
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }
    }
}


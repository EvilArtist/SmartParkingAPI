using DocumentFormat.OpenXml.Packaging;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingExcel;
using System;

namespace ApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"C:\temp\test.xlsx";
            using SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false);
            ImportDataService importDataService = new();
            var employees = importDataService.ParseData<EmployeeDataImport>(document, "Employees", true);
        }
    }
}

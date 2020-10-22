using ClosedXML.Excel;
using JLGProcessPortal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JLGProcessPortal.Controllers.ExcelSheet.Emailer
{
    public class ExcelSheetLoader
    {
      
        public IEnumerable<ExcelWorksheet> LoadWorkSheet(string xlsxDocwithPath)
        {

            //xlsxDocwithPath = @"C:\Mail\sample1.xlsx";
            var dataList = new EmailParameters();
            if (!string.IsNullOrEmpty(xlsxDocwithPath) && xlsxDocwithPath!=" ")
            {
             
                var rowData = new List<ExcelWorksheet>();
                try
                {
                    using (var wb = new XLWorkbook(xlsxDocwithPath))
                    {

                        var ws = wb.Worksheet(1);
                        var firstRowUsed = ws.FirstRowUsed();
                        var row = firstRowUsed.RowUsed();
                        var lastPossibleAddress = ws.LastCellUsed().Address;
                        var headerRow = firstRowUsed.RowUsed();
                        int columnIndex = 1;
                        int rowIndex = 1;
                        DateTime tempField;
                        while (!row.Cell(1).IsEmpty() && rowIndex <= lastPossibleAddress.RowNumber)
                        {
                            columnIndex = 1;
                            var columnData = new List<WorksheetData>();
                            while (columnIndex <= lastPossibleAddress.ColumnNumber)
                            {

                                string data = row.Cell(columnIndex).GetString();
                                if (string.IsNullOrEmpty(data))
                                    return null;
                                else if (DateTime.TryParse(data, out tempField))
                                    data = tempField.ToString("MM/dd/yyyy");

                                columnData.Add(new WorksheetData { Column = data, ColumnIndex = columnIndex });
                                columnIndex++;
                            }

                            rowData.Add(new ExcelWorksheet { RowIndex = rowIndex, Columns = columnData });
                            dataList.RecipientsList = rowData;
                            row = row.RowBelow();
                            rowIndex++;
                        }
                    }
                }
                catch(Exception)
                {
                 
                }
            }
            return dataList.RecipientsList;
        }
    }
}

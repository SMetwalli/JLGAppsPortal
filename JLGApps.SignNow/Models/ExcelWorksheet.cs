﻿using System.Collections.Generic;

namespace JLGProcessPortal.Models
{
    public class ExcelWorksheet
    {
        public int RowIndex { get; set; }
        public List<WorksheetData> Columns { get; set; }
        
    }

    public class WorksheetData
    {
        public int ColumnIndex { get; set; }
        public string Column { get; set; }
    }
}

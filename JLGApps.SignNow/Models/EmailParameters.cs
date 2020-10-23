using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models
{
    public class EmailParameters
    {
        public string Recipient { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string ExcelFile { get; set; }
        public IEnumerable<ExcelWorksheet> RecipientsList { get; set; }
    }


 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.ViewModels
{
    public class EmailLogsViewModel
    {
       public List<EmailLogs> Logs { get; set; }
        public List<SelectionList> StatusSelectionList {get;set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string SelectedSearchMethod { get; set; }
       
        public string SearchType { get; set; }

        public string CheckBoxStatusSelection { get; set; }
        public string FaileEmails { get; set; }

        public string SearchValue { get; set; }
    }

   public class SelectionList
    {
        public string SearchMethod { get; set; }
    }
    public class EmailLogs
    {
        public string Date { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }

        public string Sender { get; set; }
    }
}

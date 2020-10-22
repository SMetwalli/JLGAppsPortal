using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGProcessPortal.Models.EmailLogs
{
    public class Logs
    {
        public List<EmailListings> items { get; set; }
        public PagingLogs paging { get; set; }

    }

    public class PagingLogs
    {
        public string next { get; set; }
        public string previous { get; set; }
    }

    public class EmailListings
    {
        public string severity { get; set; }
        public EmailHeader message { get; set; }
        public string recipient { get; set; }
        public string timestamp { get; set; }

        public string @event { get; set; }

    }

    public class EmailHeader
    {
        public Email headers { get; set; }
    }
    public class Email
    {
        public string to { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
    }
}

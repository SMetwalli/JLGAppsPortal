using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models
{
    public class MassMailerDataViewModel
    {
        public string xslxFile { get; set; }
        public string emailBody { get; set; }
        public string emailSubject { get; set; }
        public string emailSender { get; set; }
        public string emailRecipientAddress { get; set; }
        public string txtMessage { get; set; }
        public string txtRecipient { get; set; }

        public string selectedFolder { get; set; }
    }
}

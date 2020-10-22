using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.ViewModels
{
   

    public class SMS
    {
        public string excelFilePath { get; set; }

        public string smsBody { get; set; }
        public string recipient { get; set; }
    }



    
    public class SendEmail
    {
        public string xslxFile { get; set; }
        public string emailBody { get; set; }
        public string emailSubject { get; set; }
        public string emailSender { get; set; }

        public string emailRecipientAddress { get; set; }
        public string txtMessage { get; set; }
        public string txtRecipient { get; set; }

    }
}



 
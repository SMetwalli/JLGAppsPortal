using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models
{
    public class AuthenticationModel
    {

        public string SIGNNOW_API_URL { get; set; }
        public string SIGNNOW_CLIENT_ID { get; set; }
        public string SIGNNOW_SECRET { get; set; }
        public string SIGNNOW_LOGIN { get; set; }
        public string SIGNNOW_PASSWORD { get; set; }

        public string SIGNNOW_BASIC { get; set; }

        public string MAILGUN_USERNAME { get; set; }
        public string MAILGUN_KEY { get; set; }

        public string MAILGUN_URL { get; set; }

        public string TWILIO_ID { get; set; }

        public string TWILIO_TOKEN { get; set; }


    }
}

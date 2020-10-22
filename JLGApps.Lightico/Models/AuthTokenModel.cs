using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.Models
{
    public class AuthToken
    {
       
            public string access_token { get; set; }
            public string refresh_token { get; set; }

            public int expires_in { get; set; }

            public string error { get; set; }
   
    }

    public class AuthenticationModel
    {


        public string LIGHTICO_API_URL { get; set; }
        public string LIGHTICO_LOGIN { get; set; }
        public string LIGHTICO_SECRET { get; set; }
        public string LIGHTICO_PASSWORD { get; set; }
        public string LIGHTICO_CLIENT_ID { get; set; }
        public string LIGHTICO_BASIC { get; set; }

        public string MAILGUN_USERNAME { get; set; }
        public string MAILGUN_KEY { get; set; }

        public string MAILGUN_URL { get; set; }

        public string TWILIO_ID { get; set; }

        public string TWILIO_TOKEN { get; set; }


    }
}

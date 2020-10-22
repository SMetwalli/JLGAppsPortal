using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace JLGApps.Lightico.Controllers.MessagingService
{
    public interface IMessaging
    {
        void SendSMS(IDictionary<string, string> smsParameters);
        IRestResponse SendEmail(IDictionary<string, string> emailParameters);
    }
    public class Messaging : IMessaging
    {
        public IRestResponse SendEmail(IDictionary<string, string> emailParameters)
        {
            string recipient = emailParameters["EMAIL_RECIPIENT"].ToString();
            //string filePath = emailParameters["FILE_PATH"].ToString();
          
            string message = emailParameters["EMAIL_BODY"].ToString();
            string subject = emailParameters["EMAIL_SUBJECT"].ToString();
            string from = emailParameters["EMAIL_SENDER"].ToString();
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-4a19e783bf5912dfe99c3dd97744b882");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "johnsonlawgroup.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", string.Concat("Johnson Law Group <",from.Trim(),">"));
            request.AddParameter("to", recipient);
            request.AddParameter("subject", subject);
            request.AddParameter("html", message);
            //request.AddFile("attachment", filePath.Trim());
            request.Method = Method.POST;
            return client.Execute(request);
        }
        public void SendSMS(IDictionary<string, string> smsParameters)
        {
            string body =  smsParameters["SMS_BODY"].ToString();
            string phoneNumber = smsParameters["SMS_RECIPIENT"].ToString();
            const string accountSid = "AC141b1cbd2d307bb249ab6af435e12c31";
            const string authToken = "0538ddcedab0594ebf6aa8f2b53702d4";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber("+12029536546"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );

            return;
        }
    }

   
}

using JLGApps.SignNow.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace JLGApps.SignNow.Controllers.MessagingService
{
    public interface IMessaging
    {
        void SendSMS(IDictionary<string, string> smsParameters);
        IRestResponse SendEmail(IDictionary<string, string> emailParameters);
    }
    public class Messaging : IMessaging
    {
        private AuthenticationModel _authConfiguration;
        public Messaging(AuthenticationModel authConfiguration)
        {
            _authConfiguration = authConfiguration;
        }

        public IRestResponse SendEmail(IDictionary<string, string> emailParameters)
        {
            string recipient = emailParameters["EMAIL_RECIPIENT"].ToString();
            //string filePath = emailParameters["FILE_PATH"].ToString();
          
            string message = emailParameters["EMAIL_BODY"].ToString();
            string subject = emailParameters["EMAIL_SUBJECT"].ToString();
            string from = emailParameters["EMAIL_SENDER"].ToString();
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(_authConfiguration.MAILGUN_URL);
            client.Authenticator = new HttpBasicAuthenticator(_authConfiguration.MAILGUN_USERNAME, _authConfiguration.MAILGUN_KEY);
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
            try
            {
                string body = smsParameters["SMS_BODY"].ToString();
                string phoneNumber = smsParameters["SMS_RECIPIENT"].ToString();
                var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber("+12029536546"),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
            }catch(Exception ex)
            {
              
            }
            return;
        }
    }

   
}

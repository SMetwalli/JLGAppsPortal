using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using JLGProcessPortal.Models.EmailLogs;
using JLGProcessPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;

namespace JLGProcessPortal.Controllers
{
    public class LogsController : Controller
    {
        [HttpGet]
        [Route("Logs")]
        [Route("Logs/Index")]
        public IActionResult Index()
        {
            var emailResponse = new EmailLogsViewModel();
            var satusSelectionList = new List<SelectionList>();

            satusSelectionList.Add(new SelectionList { SearchMethod = "Recipient" });
            satusSelectionList.Add(new SelectionList { SearchMethod = "Subject" });
            emailResponse.StatusSelectionList = satusSelectionList;
            return View(emailResponse);
        }

        [HttpPost]
        [Route("Logs/Index/{searchType?}")]
        public IActionResult Index(string searchType, string searchValue,string emailStatusType,string startDateFilter,string endDateFilter)
        {
            var response = new Logs();
            var nextResponse = new Logs();
            var emailResponse = new EmailLogsViewModel();
            var emailLogs = new List<EmailLogs>();
            var satusSelectionList = new List<SelectionList>();
            satusSelectionList.Add(new SelectionList { SearchMethod = "Recipient" });
            satusSelectionList.Add(new SelectionList { SearchMethod = "Subject" });
            emailResponse.StatusSelectionList = satusSelectionList;
            string jsonResponse = "";
            emailResponse.StartDate = startDateFilter;
            emailResponse.SearchType = searchType;
            emailResponse.EndDate = endDateFilter;
            emailResponse.SearchValue = searchValue;
            emailResponse.SelectedSearchMethod = searchType;

            DateTime startTime, endTime;

            startTime = Convert.ToDateTime(startDateFilter);

            endTime = Convert.ToDateTime(endDateFilter);
            switch (searchType)
            {

                case "Subject":
                    if (emailStatusType == "Failed")
                    {
                        jsonResponse = GetLogsBySubjectFailed(searchValue, startTime, endTime.AddHours(23).AddMinutes(59)).Content.ToString();
                        if (!string.IsNullOrEmpty(jsonResponse))
                        {
                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
                            {
                                var deserializer = new DataContractJsonSerializer(typeof(Logs));
                                response = (Logs)deserializer.ReadObject(ms);

                            }
                        }
                    }
                    else if (emailStatusType == "Delivered")
                    {

                        jsonResponse = GetLogsBySubjectDelivered(searchValue, "", startTime, endTime.AddHours(23).AddMinutes(59)).Content.ToString();

                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
                        {
                            var deserializer = new DataContractJsonSerializer(typeof(Logs));
                            response = (Logs)deserializer.ReadObject(ms);
                            if (response.items.Count < 1)
                                break;

                            var startTimeStamp = response.items.OrderByDescending(num => num.timestamp).FirstOrDefault();

                            string nextPageUrl = response.paging.next;
                            while (true)
                            {
                                double lastResponseTimestamp = Convert.ToDouble(startTimeStamp.timestamp);
                                startTime = UnixTimeConversion(lastResponseTimestamp);
                                var nextJsonResponse = GetLogsBySubjectDelivered(searchValue, nextPageUrl, startTime, endTime.AddHours(23).AddMinutes(59)).Content.ToString();

                                using (var msNext = new MemoryStream(Encoding.Unicode.GetBytes(nextJsonResponse)))
                                {
                                    var nextDeserializer = new DataContractJsonSerializer(typeof(Logs));
                                    nextResponse = (Logs)nextDeserializer.ReadObject(msNext);
                                    startTimeStamp = nextResponse.items.OrderByDescending(num => num.timestamp).FirstOrDefault();
                                    if (nextResponse.items.Count < 1)
                                        break;
                                    response.items.AddRange(nextResponse.items);
                                    nextPageUrl = nextResponse.paging.next;
                                    nextResponse.items = null;
                                    nextJsonResponse = null;
                                    nextResponse.paging.next = null;
                                }

                            }
                        }
                    }

                    break;

                case "Recipient":
                    if (emailStatusType == "Failed")
                    {
                        jsonResponse = GetLogsByRecipientFailed(searchValue, startTime, endTime.AddHours(23).AddMinutes(59)).Content.ToString();
                        if (!string.IsNullOrEmpty(jsonResponse))
                        {
                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
                            {
                                var deserializer = new DataContractJsonSerializer(typeof(Logs));
                                response = (Logs)deserializer.ReadObject(ms);

                            }
                        }
                    }
                    else if (emailStatusType == "Delivered")
                    {
                        jsonResponse = GetLogsByRecipientDelivered(searchValue, startTime, endTime.AddHours(23).AddMinutes(59)).Content.ToString();
                        if (!string.IsNullOrEmpty(jsonResponse))
                        {
                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
                            {
                                var deserializer = new DataContractJsonSerializer(typeof(Logs));
                                response = (Logs)deserializer.ReadObject(ms);

                            }
                        }
                    }
                    break;
            }

           

            if (response.items != null)
            {
                IEnumerable<EmailListings> emailEvents = new List<EmailListings>();
                if (searchType=="Subject")
                    emailEvents = response.items.Where(p => p.message.headers.subject.Trim() == searchValue.Trim());
                else if(searchType=="Recipient")
                    emailEvents = response.items.Where(p => p.message.headers.to.Trim() == searchValue.Trim());

                foreach (var email in emailEvents.OrderByDescending(d=>d.timestamp))
                {
                    string status = email.@event.ToString();
                    DateTime eventDate = UnixTimeConversion(Convert.ToDouble(email.timestamp.ToString()));
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    DateTime emailSentTime = TimeZoneInfo.ConvertTimeFromUtc(eventDate, cstZone);
                    string date = emailSentTime.ToString("M/dd/yyyy hh:mm tt");
                    emailLogs.Add(new EmailLogs()  { Date = date, Recipient = email.message.headers.to, Status = status, Subject = email.message.headers.subject, Sender = email.message.headers.from });
                }
            }

            emailResponse.Logs = emailLogs;

            return View(emailResponse);
        }

       
        public static IRestResponse GetLogsBySubjectFailed(string emailSubject,DateTime startDate,DateTime stopDate)
        {

            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-4a19e783bf5912dfe99c3dd97744b882");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "johnsonlawgroup.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/events";
            string beginDate = startDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            string endDate = stopDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            request.AddParameter("begin", beginDate);
            request.AddParameter("end", endDate);
            request.AddParameter("ascending", "yes");          
            request.AddParameter("event", "failed");            
            request.AddParameter("subject", emailSubject.Trim());
            request.AddParameter("severity", "permanent");

            return client.Execute(request);
        }

        public static IRestResponse GetLogsBySubjectDelivered(string emailSubject,string pagingSignature, DateTime startDate, DateTime stopDate)
        {

            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-4a19e783bf5912dfe99c3dd97744b882");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "johnsonlawgroup.com", ParameterType.UrlSegment);
            if (pagingSignature == "")
                request.Resource = ("{domain}/events");
            else
                request.Resource = pagingSignature;

            string beginDate = startDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            string endDate = stopDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            request.AddParameter("begin", beginDate);
            request.AddParameter("end", endDate);           
            request.AddParameter("ascending", "yes");
            //request.AddParameter("limit", 300);
            request.AddParameter("event", "delivered");
            request.AddParameter("subject", emailSubject.Trim());
          

            return client.Execute(request);
        }
        public static IRestResponse GetLogsByRecipientFailed(string recipient, DateTime startDate, DateTime stopDate)
        {

            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-4a19e783bf5912dfe99c3dd97744b882");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "johnsonlawgroup.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/events";
            string beginDate = startDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            string endDate = stopDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            request.AddParameter("begin", beginDate);
            request.AddParameter("end", endDate);
            request.AddParameter("ascending", "yes");
            //request.AddParameter("limit", 300);
            request.AddParameter("event", "failed");
            request.AddParameter("recipient", recipient.Trim());
            request.AddParameter("severity", "permanent");

            return client.Execute(request);
        }

        public static IRestResponse GetLogsByRecipientDelivered(string recipient, DateTime startDate, DateTime stopDate)
        {

            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-4a19e783bf5912dfe99c3dd97744b882");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "johnsonlawgroup.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/events";
            string beginDate = startDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            string endDate = stopDate.ToString("ddd, dd MMM yyy HH:mm:ss CST");
            request.AddParameter("begin", beginDate);
            request.AddParameter("end", endDate);
            request.AddParameter("ascending", "yes");
            //request.AddParameter("limit", 300);
            request.AddParameter("event", "delivered");
            request.AddParameter("recipient", recipient.Trim());
           

            return client.Execute(request);
        }
        public static DateTime UnixTimeConversion(double unixTime)
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);   
                return epoch.AddSeconds(unixTime);
            }
        }
}

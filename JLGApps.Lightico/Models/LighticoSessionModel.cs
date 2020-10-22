using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.Models
{
    public class LighticoSessionRequestModel
    {
        public string sourceName { get; set; }
        public string userName { get; set; }
        public string customerName { get; set; }
        public string email { get; set; }

        public bool sendNow { get; set; }
        public Customer customerData { get; set; }
    }

    public class DocumentRequestModel
    {
        public string TemplateId { get; set; }
        public string SessionId { get; set; }
        public string name { get; set; }

        public Customer customerData { get; set; }


    }
    
    public class Customer
    {
        public string mergeField { get; set; }
        public string mergeFieldValue { get; set; }
    }

    public class LighticoSessionResponseModel
    {
        public string agentURL { get; set; }
        public string customerId { get; set; }
        public string sessionId { get; set; }
        public string customerURL { get; set; }

        public string error { get; set; }

    }

    public class DocumentResponseModel
    {
        public string esignId { get; set; }
        public string error { get; set; }
    }
}

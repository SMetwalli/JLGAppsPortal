using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Controllers.ApiCalls
{
    public interface ISignNowDocumentCreator
    {
        string GenerateDocument(string accessToken, string apiUrl, DocumentParameters document);
        string MoveDocument(string accessToken, string apiUrl, DocumentParameters document);

        void MergeSmartFields(string accessToken, string apiUrl, string smartFields, DocumentParameters document);

        dynamic GenerateLink(string accessToken, string apiUrl, DocumentParameters document);
    }
    public class SignNowDocumentCreator: ISignNowDocumentCreator
    {
        public  string GenerateDocument(string accessToken, string apiUrl, DocumentParameters document)
        {
            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };

            var request = new RestRequest($"/template/{document.TemplateId}/copy", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddJsonBody(document);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                results = response.Content.ToString();
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            return results;
        }

        public  string MoveDocument(string accessToken, string apiUrl, DocumentParameters document)
        {
            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };

            var request = new RestRequest($"/document/{document.id}/move", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddJsonBody(document);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                results = response.Content.ToString();
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            return results;
        }
        public  void MergeSmartFields(string accessToken, string apiUrl, string smartFields, DocumentParameters document)
        {
                dynamic results = "";
                var client = new RestClient { BaseUrl = new Uri(apiUrl) };

                var request = new RestRequest($"/document/{document.id}/integration/object/smartfields", Method.POST)
                    .AddHeader("Accept", "application/json")
                    .AddHeader("Authorization", $"Bearer {accessToken}")
                    .AddJsonBody(smartFields);

                var response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    Console.WriteLine(response.Content.ToString());
                    results = response.Content.ToString();
                }
                

        }


        public  dynamic GenerateLink(string accessToken, string apiUrl, DocumentParameters document)
        {
            StringBuilder hyperLink = new StringBuilder();
            string signNowlink = hyperLink.Append("{").Append('"').Append("document_id").Append('"').Append(':').Append('"').Append(document.id).Append('"').Append("}").ToString();

            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };

            var request = new RestRequest($"/link", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddJsonBody(signNowlink);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                results = response.Content.ToString();
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            Console.WriteLine(response.Content.ToString());
            return results = response.Content.ToString();

        }
    }
}

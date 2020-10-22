using JLGApps.Lightico.Models;
using JLGApps.Lightico.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JLGApps.Lightico.Controllers.LighticoApisCalls
{
    public interface IDocumentCreator
    {
        DocumentResponseModel GenerateDocument(string accessToken, string sessionId, string customerData);

        LighticoSessionResponseModel CreateNewSession(string accessToken, string customerData);
       

     
    }
    public class DocumentCreator: IDocumentCreator
    {

        private AuthenticationModel _signNowConfiguration;
        public DocumentCreator(AuthenticationModel signNowConfiguration)
        {
            _signNowConfiguration=signNowConfiguration;
        }
        
        public DocumentResponseModel GenerateDocument(string accessToken,string sessionId, string customerData)
        {
            string apiUrl = _signNowConfiguration.LIGHTICO_API_URL;
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };

            var request = new RestRequest($"/V2.3/sessions/{sessionId}/esigns", Method.POST)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddParameter("application/json", customerData, ParameterType.RequestBody);
              
             

            var response = client.Execute(request);

            dynamic results;
            if (response.StatusCode == HttpStatusCode.OK)
            {

                results = response.Content.ToString();
                return JsonSerializer.Deserialize<DocumentResponseModel>(results);
            }
            else
            {
                var docResponse = new DocumentResponseModel();
                Console.WriteLine(response.Content.ToString());
                docResponse.error = response.Content.ToString();
                return docResponse;
            }

         
        }

        public LighticoSessionResponseModel CreateNewSession(string accessToken,string customerData)
        {
            string apiUrl = _signNowConfiguration.LIGHTICO_API_URL;
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };
            client.Timeout = -1;
            var request = new RestRequest($"/v2.3/sessions", Method.POST)                
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddHeader("Content-Type", "application/json")
                .AddParameter("application/json", customerData,ParameterType.RequestBody);

            var response = client.Execute(request);

            dynamic results;
            if (response.StatusCode == HttpStatusCode.OK)
            {
            
                results = response.Content.ToString();
                return JsonSerializer.Deserialize<LighticoSessionResponseModel>(results);
             
            }
            else
            {
                var docResponse = new LighticoSessionResponseModel();
                Console.WriteLine(response.Content.ToString());
                docResponse.error = response.Content.ToString();
                return docResponse;
            }

        
        }
       


       

      
    }
}

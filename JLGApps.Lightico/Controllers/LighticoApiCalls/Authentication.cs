using JLGApps.Lightico.Models;
using RestSharp;
using System;
using System.Net;
using System.Text.Json;

namespace JLGApps.Lightico.Controllers.LighticoApis
{
    public class Authentication
    {
        private  AuthenticationModel _signNowConfiguration;

        public Authentication(AuthenticationModel signNowConfiguration)
        {
            _signNowConfiguration = signNowConfiguration;
        }
        public AuthToken GenerateToken()
        {

            string apiUrl = _signNowConfiguration.LIGHTICO_API_URL;           
            string basicKey = _signNowConfiguration.LIGHTICO_BASIC;


            
            var client = new RestClient { BaseUrl = new Uri(apiUrl) };


            var request = new RestRequest($"/v2.3/services/oauth2/token", Method.POST)                
                .AddParameter("grant_type", "client_credentials")          
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddHeader("Authorization", $"Basic {basicKey}");

           


            var response = client.Execute(request);
            dynamic results;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Content;
                return JsonSerializer.Deserialize<AuthToken>(results);
            }
            else
            {
                var authorizationResponse = new AuthToken();
                Console.WriteLine(response.Content.ToString());
                authorizationResponse.error = response.Content.ToString();
                return authorizationResponse;
              
            }

      


        

        }
    }
}

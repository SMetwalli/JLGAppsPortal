using JLGProcessPortal.Models.SignNow;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JLGProcessPortal.Controllers.Vendors.SignNow
{
    public class Authentication
    {
        public string GenerateToken(SignNowAuth _signNowConfiguration)
        {

            string baseUrl = _signNowConfiguration.SIGNNOW_API_URL;
            string userLogin = _signNowConfiguration.SIGNNOW_LOGIN;
            string userPassword = _signNowConfiguration.SIGNNOW_PASSWORD;
            string basicKey = _signNowConfiguration.SIGNNOW_BASIC;




            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(baseUrl) };


            var request = new RestRequest($"oauth2/token", Method.POST)
                .AddParameter("username", userLogin)
                .AddParameter("password", userPassword)
                .AddParameter("grant_type", "password")
                .AddParameter("scope", "*")
                .AddHeader("Content-Type", "multipart/form-data")
                .AddHeader("Authorization", $"Basic {basicKey}");

            request.AlwaysMultipartFormData = true;


            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
                results = response.Content.ToString();
            else
                results = response.Content.ToString();


            return results;


        

        }
    }
}

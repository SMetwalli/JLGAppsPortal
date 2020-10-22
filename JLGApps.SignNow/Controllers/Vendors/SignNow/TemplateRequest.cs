
using JLGProcessPortal.Controllers.Vendors.SignNow;
using JLGProcessPortal.Models.SignNow;
using JLGProcessPortal.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace JLGProcessPortal.Controllers.Vendors.SignNow
{
    public class SignNowTemplateRequest: ISignNow
    {
        public List<TemplateInfo> GetTemplates(SignNowAuth authentication,string folderName,string folderId)
        {
          
                var templateJson = GetTemplateInfo(authentication, folderId);
                var templateParameters = JsonSerializer.Deserialize<TemplateResults>(templateJson);

             

            var templateData = new List<TemplateInfo>();
          

            foreach (var template in templateParameters.documents)
            {
                templateData.Add(new TemplateInfo { Name = template.document_name, templateFolderID = template.id, folderId= folderId, folderName= folderName });
            }
            return templateData;
        }


        public string GetTemplateInfo(SignNowAuth authentication,string folderId)
        {
            string baseUrl = "https://api.signnow.com/";
            var SignNowAuthentication = new Authentication();
            string accessToken = SignNowAuthentication.GenerateToken(authentication);
            var token = JsonSerializer.Deserialize<AuthToken>(accessToken);
            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(baseUrl) };

            var request = new RestRequest($"folder/{folderId}?with_team_documents =true", Method.GET)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", $"Bearer {token.access_token}");


            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
                results = response.Content.ToString();
            else
                results = response.Content.ToString();


            return results;
        }


        public FolderList GetFolders(SignNowAuth authentication)
        {
            string baseUrl = "https://api.signnow.com/";
            var SignNowAuthentication = new Authentication();
            string accessToken = SignNowAuthentication.GenerateToken(authentication);
            var token = JsonSerializer.Deserialize<AuthToken>(accessToken);
            dynamic results = "";
            var client = new RestClient { BaseUrl = new Uri(baseUrl) };

            var request = new RestRequest($"user/folder/", Method.GET)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", $"Bearer {token.access_token}");


            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Content.ToString();


                var templateFolders = JsonSerializer.Deserialize<FolderList>(results);

                return templateFolders;

            }

            else
                results = response.Content.ToString();


            return results;


        }
    }
}

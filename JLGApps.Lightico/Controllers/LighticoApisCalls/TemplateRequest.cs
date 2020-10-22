using JLGApps.Lightico.Controllers.LighticoApis;
using JLGApps.Lightico.Models;
using JLGApps.Lightico.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JLGApps.Lightico.Controllers.LighticoApisCalls
{
    public interface ILigticoTemplates
    {
        List<TemplateAttributes> GetTemplates(List<FolderList> folders, string folderName, string folderId);
      

        List<FolderList> GetFolders(LighticoAuthorizationModel authentication);
    }
    public class Templates : ILigticoTemplates
    {
        private LighticoAuthorizationModel _signNowConfiguration;

        public Templates(LighticoAuthorizationModel signNowConfiguration)
        {
            _signNowConfiguration = signNowConfiguration;
        }

        public List<TemplateAttributes> GetTemplates(List<FolderList> folders,string folderName,string folderId)
        {
          

            var templateData = new List<TemplateAttributes>();
          

            foreach (var template in folders.Where(r => r.Path == folderId).ToList())
            {
                templateData.Add(new TemplateAttributes { Name = template.Name, templateFolderID = template.Id, folderId= folderId, folderName= folderName });
            }
            return templateData;
        }


     


        public List<FolderList> GetFolders(LighticoAuthorizationModel authentication)
        {
            string apiUri = _signNowConfiguration.LIGHTICO_API_URL;
            var lighticoAuthentication = new Authentication(_signNowConfiguration);
            var token = lighticoAuthentication.GenerateToken();


            var client = new RestClient { BaseUrl = new Uri(apiUri) };

            var request = new RestRequest($"esigns/", Method.GET)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", $"Bearer {token.access_token}");
               


            var response = client.Execute(request);
            var folderList = new List<FolderList>();
            dynamic results = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Content.ToString();
                var templateFolders = JsonConvert.DeserializeObject<Folders[]>(results);
                foreach (var folder in templateFolders)
                {
                    folderList.Add(new FolderList { Id = folder.Id, Name = folder.Name, Path = folder.Path });
                }
                return folderList;
            }
            else
            {
                folderList.FirstOrDefault().Error = response.Content.ToString();
                return folderList.ToList();
            }


          


        }
    }
}

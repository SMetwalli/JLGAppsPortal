using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models.SignNow
{
    public class TemplateResults
    {
        public List<DocumentList> documents { get; set; }
     
    }

    public class DocumentList
    {
        public string id { get; set; }
        public string document_name { get; set; }
    }


   public class FolderList
    {
        public List<Folder> folders { get; set; }
    }

    public class Folder
    { 
        public string id { get; set; }
        public string name { get; set; }

        public string team_name { get; set; }
    }
}

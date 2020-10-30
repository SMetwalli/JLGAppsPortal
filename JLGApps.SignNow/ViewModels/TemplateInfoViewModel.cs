using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.ViewModels
{
    public class TemplateInfo
    {
        public string Name { get; set; }
        public string templateID { get; set; }

        public string templateName { get; set; }

        public string folderId { get; set; }

        public string folderName { get; set; }
        public List<MergeFields> Fields { get; set; } 
    }


    public class MergeFields
    {
        public string fieldValue { get; set; }
    }


    public class TemplateRequestParameters
    {
        public string FolderId { get; set; }

       public string FolderName { get; set; }
        public string casenumber { get; set; }
    }
   

   
}

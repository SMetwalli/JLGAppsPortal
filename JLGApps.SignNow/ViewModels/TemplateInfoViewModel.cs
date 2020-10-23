using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.ViewModels
{
    public class TemplateInfo
    {
        public string Name { get; set; }
        public string templateFolderID { get; set; }

        public string templateName { get; set; }

        public string folderId { get; set; }

        public string folderName { get; set; }
        public List<MergeFields> Fields { get; set; } 
    }


    public class MergeFields
    {
        public string fieldValue { get; set; }
    }


   

   
}

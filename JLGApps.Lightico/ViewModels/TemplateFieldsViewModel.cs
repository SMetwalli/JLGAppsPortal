using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.ViewModels
{
   




    public class LighticoTemplateFields
    {
   

        public List<string> smartFields { get; set; }
        public List<string> mergeHeaders { get; set; }

        public string sessionId { get; set; }
        public string templateID { get; set; }

        public string templateName { get; set; }

        public string caseNumber { get; set; }

        public string fullName { get; set; }

        public string excelFilePath { get; set; }

        public string templateFolderId { get; set; }
    }

    

}



 
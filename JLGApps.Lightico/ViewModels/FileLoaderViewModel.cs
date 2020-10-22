using JLGApps.Lightico.Models;
using System.Collections.Generic;

namespace JLGApps.Lightico.ViewModels
{
    public class FileLoaderViewModel
    {
        
        public string SMSBody { get; set; }
        public string SMSRecipient { get; set; }
        public string FileAndPath { get; set; }
        public string EmailBody { get; set; }

        public string EmailSubject { get; set; }

        public string EmailSender { get; set; }

        public string EmailRecipient { get; set; }
        public IEnumerable<ExcelWorksheet> Recipients { get; set; }

        public List<Mailboxes> MailBoxes { get; set; }

        public List<TemplateAttributes> MultipleTemplateProfiles { get; set; }

        public TemplateAttributes SingleTemplateProfile { get; set; }


        public List<FolderList> TemplateFolderList { get; set; }

        public string SelectedTemplateFolder { get; set; }

        public string CaseNumberExcelHeader { get; set; }

        public string FullNameExcelHeader { get; set; }

    }
}

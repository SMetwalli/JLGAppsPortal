using JLGApps.SignNow.Models;
using JLGApps.SignNow.Models.SignNow;
using System.Collections.Generic;

namespace JLGApps.SignNow.ViewModels
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

        public List<TemplateInfo> MultipleTemplateProfiles { get; set; }

        public TemplateInfo SingleTemplateProfile { get; set; }


        public FolderList TemplateFolderList { get; set; }

        public string SelectedTemplateFolder { get; set; }

        public string CaseNumberExcelHeader { get; set; }

    }
}

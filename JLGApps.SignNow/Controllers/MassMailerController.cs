
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;
using ClosedXML.Excel;
using Microsoft.Extensions.Options;
using System.Text;
using JLGApps.SignNow.Controllers.ApiCalls;
using JLGApps.SignNow.Models;
using JLGApps.SignNow.ViewModels;
using JLGApps.SignNow.Models.SignNow;
using JLGApps.SignNow.Controllers.MessagingService;
using JLGApps.SignNow.Controllers.ExcelSheet.Emailer;
using System.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JLGApps.SignNow.Controllers
{

    public class MassMailerController : Controller
    {
        private AuthenticationModel _authConfiguration;
        private readonly EmailTemplateDetailsContext _context;
        public MassMailerController(IOptions<AuthenticationModel> authConfiguration, EmailTemplateDetailsContext context)
        {
            _authConfiguration = authConfiguration.Value;
            _context = context; ;
        }

        [Route("")]
        [Route("MassMailer")]
        [Route("MassMailer/Index")]
        public IActionResult Index()
        {
            var mailboxes = new MailboxSelection();
            var mailboxesList = mailboxes.GetMailboxes();
            ISignNow templates = new SignNowTemplateRequest();
            

            var templateFolders = templates.GetFolders(_authConfiguration);
          

            var envelope = new FileLoaderViewModel
            {

                Recipients = null,
                FileAndPath = " ",
                SMSRecipient=" ",
                SMSBody= " ",
                EmailBody = "<p><br></p>",
                EmailSubject = "",
                EmailSender = "",
                EmailRecipient = "",
                MailBoxes = mailboxesList,               
                TemplateFolderList= templateFolders


            };
         

            ViewBag.MessageExist = "False";
            return View(envelope);
        }
        

        [Route("MassMailer/Index/{excelFile?}")]
        public ActionResult Index(string excelFile, string emailBody, string emailSubject, string emailSender, string emailRecipient,string messageAlert,string templateID,
                                    string txtMessage,string txtRecipient, string selectedFolder, string folderId,string casenumExcelHeader)
        {
            var mailboxes = new MailboxSelection();
            var mailboxesList = mailboxes.GetMailboxes();
            var RecipientLoader = new ExcelSheetLoader();
            var envelope = new FileLoaderViewModel();
            ISignNow templates = new SignNowTemplateRequest();
            var temp = new List<MergeFields>();
            if (folderId != null && folderId!="NONE")
            {
                var templateProfile = templates.GetTemplates(_authConfiguration, selectedFolder.Trim(), folderId);

                var singleTempProfile = templateProfile.Where(t => t.templateFolderID == templateID).FirstOrDefault();
                if (singleTempProfile != null)
                    envelope.SingleTemplateProfile = singleTempProfile;
                if (templateProfile != null)
                    envelope.MultipleTemplateProfiles = templateProfile;
            }
            var templateFolders = templates.GetFolders(_authConfiguration);

            if (!string.IsNullOrEmpty(excelFile))
            {
                var recipientsCollection = RecipientLoader.LoadWorkSheet(excelFile);
                if (recipientsCollection != null)
                    envelope.Recipients = recipientsCollection;
                else
                { messageAlert = string.Concat(messageAlert, "\n\rMissing or incomplete data in excel worksheet!!"); }
            }

            if(excelFile!=null)
                envelope.FileAndPath = excelFile.Trim();
            if(emailBody!=null)
                envelope.EmailBody = emailBody.Trim();
            if(emailSubject!=null)
                envelope.EmailSubject = emailSubject.Trim();
            if(emailSender!=null)
                envelope.EmailSender = emailSender.Trim();
            if(emailRecipient!=null)
                envelope.EmailRecipient = emailRecipient.Trim();
            if(mailboxesList!=null)
                envelope.MailBoxes = mailboxesList;
           
            if (templateFolders != null)
                envelope.TemplateFolderList = templateFolders;

            if (selectedFolder != null)
                envelope.SelectedTemplateFolder = selectedFolder;
            if (casenumExcelHeader != null)
                envelope.CaseNumberExcelHeader = casenumExcelHeader;

            if (!string.IsNullOrEmpty(txtMessage))
                envelope.SMSBody = txtMessage;

            if (!string.IsNullOrEmpty(txtRecipient))
                envelope.SMSRecipient = txtRecipient;

            TempData["EXCEL_FILE"] = excelFile;
            ViewBag.EmailBody = emailBody;

            if (messageAlert != " " && messageAlert != null)
            { ViewBag.MessageAlert = messageAlert; ViewBag.MessageExist = "True"; }
            else
            { ViewBag.MessageExist = "False"; }


            return View(envelope);
        }

       

        [HttpPost]
        [Route("MassMailer/SelectedTemplateFolder/{xslxFile?}")]
        public ActionResult SelectedTemplateFolder(string xslxFile, string emailBody, string emailSubject, string emailSender, string emailRecipientAddress, string txtMessage,
                                                   string txtRecipient,string selectedFolder,string folderId,string casenumberExcelHeader)
        {
            string excelFile = " ";
            string body = " ";
            string subject = " ";
            string sender = " ";
            string recipient = " ";
            string smsMessage = " ";
            string smsRecipient = " ";
            string casenumExcelHeader = " ";

            if (!string.IsNullOrEmpty(emailBody))
                body = emailBody.Trim();
            if (!string.IsNullOrEmpty(emailSubject))
                subject = emailSubject.Trim();
            if (!string.IsNullOrEmpty(emailSender))
                sender = emailSender.Trim();
            if (!string.IsNullOrEmpty(emailRecipientAddress))
                recipient = emailRecipientAddress.Trim();
            if (!string.IsNullOrEmpty(xslxFile))
                excelFile = xslxFile.Trim();
            if (!string.IsNullOrEmpty(txtMessage))
                smsMessage = txtMessage.Trim();
            if (!string.IsNullOrEmpty(txtRecipient))
                smsRecipient = txtRecipient.Trim();
            if (!string.IsNullOrEmpty(casenumberExcelHeader))
                casenumExcelHeader = casenumberExcelHeader.Trim();

            return RedirectToAction("Index", new
            {
                excelFile = excelFile,
                emailBody = body,
                emailSubject = subject,
                emailSender = sender,
                emailRecipient = recipient,
                emailSentToRecipient = false,
                messageAlert = " ",
                templateID = "",
                txtMessage = smsMessage,
                txtRecipient = smsRecipient,
                selectedFolder= selectedFolder,
                folderId= folderId,
                casenumExcelHeader = casenumberExcelHeader

            });
        }


        [HttpPost]  
        [Route("MassMailer/LoadData/{xslxFile?}")]
        public ActionResult LoadDataAsync()
        {
            string path = @"C:\Bulk Mailer";
            string excelFile = " ";
            string fileName, messageAlert;
            var envelope = new FileLoaderViewModel();

            if (Request.Form.Files != null)
            {
                var file = Request.Form.Files[0];
                fileName = file.FileName;
              

                try
                {
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        if (Directory.Exists(path) == true)
                            file.CopyTo(fileStream);
                    }
                    excelFile = Path.Combine(path, file.FileName);
                    
                    var RecipientLoader = new ExcelSheetLoader();
                    var excelRecipients = RecipientLoader.LoadWorkSheet(excelFile);

                    if (excelRecipients != null)
                    {
                        envelope.Recipients = excelRecipients;
                        return Json(new { recipients = envelope.Recipients });
                    }

                }
                catch (Exception ex)
                {

                    if (ex.HResult == -2147024864)
                        messageAlert = "This excel worksheet cannot be loaded because it is already open and running.\n\r " +
                            "More than one of the same excel worksheet cannot be running at the same time.\n\r  " +
                            "Please close and shut down its process.";

                    messageAlert = ex.ToString();
                }
            }
            envelope.Recipients = null;
            return Json(new { recipients = envelope });
        }
     
        
        [HttpPost]      
        [Route("MassMailer/SendEmail/{emailParameters?}")]
        public ActionResult SendEmail([FromBody] SendEmail emailParameters)
         {
            const string  repositoryPath = "C:\\Bulk Mailer\\";
            //string recipient = "kwilliams@johnsonlawgroup.com";
            dynamic results = "";
            string excelFile = "";
            string body = "";
            string subject = "";
            string sender = "";
            string recipient = "";           
            string messageAlert = " ";
            string smsMessage = " ";
            string smsRecipient = " ";

            if (!string.IsNullOrEmpty(emailParameters.emailBody) && emailParameters.emailBody != "<p><br></p>")
            { body = emailParameters.emailBody.Trim(); }
            else
            { messageAlert = "Cannot send a empty email body message."; }

            if (!string.IsNullOrEmpty(emailParameters.emailSubject))
                subject = emailParameters.emailSubject.Trim();
            if (!string.IsNullOrEmpty(emailParameters.emailSender))
                sender = emailParameters.emailSender.Trim();
            if (!string.IsNullOrEmpty(emailParameters.emailRecipientAddress))
                recipient = emailParameters.emailRecipientAddress.Trim();
            if (!string.IsNullOrEmpty(emailParameters.txtMessage))
                smsMessage = emailParameters.txtMessage.Trim();
            if (!string.IsNullOrEmpty(emailParameters.txtRecipient))
                smsRecipient = emailParameters.txtRecipient.Trim();

            var email = new Dictionary<string, string>();

            email.Add("EMAIL_BODY", body);
            email.Add("EMAIL_SUBJECT", subject);
            try
            {
                var RecipientLoader = new ExcelSheetLoader();
                var recipientEmail = new EmailParameters();
                if (!string.IsNullOrEmpty(emailParameters.xslxFile))
                {
                    excelFile = Path.Combine(repositoryPath, emailParameters.xslxFile);
                    var recipientsCollection = RecipientLoader.LoadWorkSheet(excelFile.Trim());
                    if (recipientsCollection != null)
                        recipientEmail.RecipientsList = recipientsCollection;
                    else
                    { messageAlert = string.Concat(messageAlert, "\n\rMissing or incomplete data in excel worksheet!!"); }
                }



                if (recipientEmail.RecipientsList != null && recipient.Contains("[[") && recipient.Contains("]]"))
                {

                    var mergeFields = recipientEmail.RecipientsList.Where(a => a.RowIndex == 1).FirstOrDefault();
                    if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(sender) && !string.IsNullOrEmpty(recipient))
                    {
                        int currentRow = 0;
                        int totalRecipientCount = recipientEmail.RecipientsList.Count() - 1;
                        string bodyTemplate = body;
                        foreach (var recipientRow in recipientEmail.RecipientsList.Skip(1))
                        {
                            currentRow++;
                            Startup.Progress = (int)((float)currentRow / (float)totalRecipientCount * 100.0);
                            body = "";
                            body = bodyTemplate;

                            foreach (var mergeField in mergeFields.Columns)
                            {
                                int headerColumnindex = mergeFields.Columns.FindIndex(field => field.Column.Contains(mergeField.Column));

                                string mergeFieldUpperCase = string.Concat("[[", mergeField.Column.ToUpper().Trim(), "]]");

                                if (body.Contains(mergeFieldUpperCase.Trim()))
                                {

                                    string fieldValue = recipientRow.Columns[headerColumnindex].Column.Replace("[[", " ").Replace("]]", " ");
                                    body = body.Replace(mergeFieldUpperCase, fieldValue.Trim());
                                }

                                //Mail merge Recipient
                                if (emailParameters.emailRecipientAddress.Contains(mergeFieldUpperCase.Trim()))
                                {
                                    string fieldValue = recipientRow.Columns[headerColumnindex].Column.Replace("[[", " ").Replace("]]", " ");
                                    recipient = emailParameters.emailRecipientAddress.Replace(mergeFieldUpperCase, fieldValue.Trim());
                                }

                            }

                            var EmailerHost = new Messaging(_authConfiguration);

                            //Dictionary for Mailgun
                            email = new Dictionary<string, string>()
                        {
                                { "EMAIL_RECIPIENT", recipient},
                                { "EMAIL_BODY", body.Trim()},
                                { "EMAIL_SUBJECT", subject},
                                { "EMAIL_SENDER", sender}
                        };
                            EmailerHost.SendEmail(email);

                            Task.Delay(2000);

                        }

                        messageAlert = "All emails have been sent to recipients.";

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(sender) && !string.IsNullOrEmpty(recipient) && !recipient.Contains("[["))
                    {
                        //VALIDATE FOR A VALID EMAIL ADDRESS
                        try
                        {
                            var EmailerHost = new Messaging(_authConfiguration);

                            //Dictionary for Mailgun
                            email = new Dictionary<string, string>()
                                {
                                    { "EMAIL_RECIPIENT", recipient},
                                    { "EMAIL_BODY", body.Trim()},
                                    { "EMAIL_SUBJECT", subject},
                                    { "EMAIL_SENDER", sender}
                                };

                            EmailerHost.SendEmail(email);
                            messageAlert = "All emails have been sent to recipients.";
                        }
                        catch (FormatException ex)
                        {

                            messageAlert = string.Concat("Emails failed to send", messageAlert);
                        }
                    }
                    else
                    {
                        messageAlert = string.Concat("Emails failed to send. ", messageAlert);
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

          
            return Json(new { apiResponse = results, message = messageAlert });
        }

      
        [HttpPost]
        [Route("MassMailer/SendSMS/{smsMessage?}")]
        public ActionResult SendSMS([FromBody] SMS smsMessage)
        {
            const string repositoryPath = "C:\\Bulk Mailer\\";
            //Load excel phonenumber and signer link
            string excelFile = "";
            string body = "";
            string recipient = "";
            string messageAlert = " ";
            if (!string.IsNullOrEmpty(smsMessage.smsBody))
                body = smsMessage.smsBody;
            if (!string.IsNullOrEmpty(smsMessage.recipient))
                recipient = smsMessage.recipient;

            var RecipientLoader = new ExcelSheetLoader();
            var recipientSMS = new EmailParameters();
            if (!string.IsNullOrEmpty(smsMessage.excelFilePath))
            {
                excelFile = Path.Combine(repositoryPath, smsMessage.excelFilePath);
                var recipientsCollection = RecipientLoader.LoadWorkSheet(excelFile.Trim());
                recipientSMS.RecipientsList = recipientsCollection;
            }

            if (smsMessage.recipient.Contains("[[") && smsMessage.recipient.Contains("]]"))
            {

                var mergeFields = recipientSMS.RecipientsList.Where(a => a.RowIndex == 1).FirstOrDefault();
                if (!string.IsNullOrEmpty(smsMessage.smsBody) && !string.IsNullOrEmpty(smsMessage.recipient))
                {
                    string bodyTemplate = body;
                    int currentRow = 0;
                    foreach (var recipientRow in recipientSMS.RecipientsList.Skip(1))
                    {
                        
                        int totalRecipientCount=recipientSMS.RecipientsList.Count()-1;
                        currentRow++;
                        Startup.Progress = (int)((float)currentRow / (float)totalRecipientCount * 100.0);
                        body = "";
                        body = bodyTemplate;

                        foreach (var mergeField in mergeFields.Columns)
                        {
                            int headerColumnindex = mergeFields.Columns.FindIndex(field => field.Column.Contains(mergeField.Column));

                            string mergeFieldUpperCase = string.Concat("[[", mergeField.Column.ToUpper().Trim(), "]]");

                            if (body.Contains(mergeFieldUpperCase.Trim()))
                            {

                                string fieldValue = recipientRow.Columns[headerColumnindex].Column.Replace("[[", " ").Replace("]]", " ");
                                body = body.Replace(mergeFieldUpperCase, fieldValue.Trim());
                            }

                            //Mail merge Recipient
                            if (smsMessage.recipient.Contains(mergeFieldUpperCase.Trim()))
                            {
                                string fieldValue = recipientRow.Columns[headerColumnindex].Column.Replace("[[", " ").Replace("]]", " ");
                                recipient = smsMessage.recipient.Replace(mergeFieldUpperCase, fieldValue.Trim());
                            }

                        }

                        var EmailerHost = new Messaging(_authConfiguration);

                        //Dictionary for Twilio
                       var  sms = new Dictionary<string, string>()
                                {
                                    { "SMS_RECIPIENT", recipient},
                                    { "SMS_BODY", body.Trim()}                                  
                                };
                        EmailerHost.SendSMS(sms);
                        Task.Delay(1000);
                        
                    }

                    messageAlert = "All sms messages have been sent to recipients.";

                }
                else
                {
                    messageAlert = "All sms messages have not been sent to recipients.";
                }
            }
            return Json(new { message=messageAlert });
        }


        [HttpPost]
        [Route("MassMailer/SignNowGenerateDocument/{signNowSmartFields?}")]
        public async Task<IActionResult> SignNowGenerateDocument([FromBody] SignNowTemplateInformation signNowTemplateFields)
        {
            string messageAlert = "";
            string templateName = signNowTemplateFields.templateName;
            string templateId = signNowTemplateFields.templateID;
            string xslxFile = signNowTemplateFields.excelFilePath.Trim();
            string folderId = signNowTemplateFields.templateFolderId;
            dynamic results = "";
            var RecipientLoader = new ExcelSheetLoader();
            var recipientEmail = new EmailParameters();
            StringBuilder jsonPayload = new StringBuilder();
            var document = new DocumentParameters
            {
                TemplateId = templateId,
                folder_id = folderId
            };
            try
            {
                if (signNowTemplateFields.caseNumber != "NONE")
                {
                    if (!string.IsNullOrEmpty(xslxFile))
                    {

                        var recipientsCollection = RecipientLoader.LoadWorkSheet(xslxFile);
                        recipientEmail.RecipientsList = recipientsCollection;
                        int totalRecipientCount = recipientEmail.RecipientsList.Count() - 1;
                        using (var wb = new XLWorkbook(xslxFile))
                        {

                            var ws = wb.Worksheet(1);
                            var firstRowUsed = ws.FirstRowUsed();
                            var row = firstRowUsed.RowUsed();
                            row = row.RowBelow();
                            var headerRow = firstRowUsed.RowUsed();
                            var lastPossibleAddress = ws.LastCellUsed().Address.ColumnNumber;
                            var signNowLinkColumn = ws.Column(lastPossibleAddress);
                            int signNowLinkColumnIndex = lastPossibleAddress;

                            if (headerRow.Cell(lastPossibleAddress).Value.ToString() != "SIGNER_LINK")
                            {
                                signNowLinkColumn.InsertColumnsAfter(1);
                                signNowLinkColumnIndex = lastPossibleAddress + 1;
                                headerRow.Cell(signNowLinkColumnIndex).Value = "SIGNER_LINK";

                                wb.SaveAs(xslxFile);
                            }
                            int currentRow = 0;
                            while (!row.Cell(1).IsEmpty())
                            {
                                jsonPayload.Append("{").Append('"').Append("data").Append('"').Append(":[");
                                int column = 1;
                                currentRow++;
                                Startup.Progress = (int)((float)currentRow / (float)totalRecipientCount * 100.0);
                                while (!row.Cell(column).IsEmpty())
                                {

                                    string columnName = headerRow.Cell(column).GetString();
                                    string data = row.Cell(column).GetString();
                                    document.document_name = string.Concat(templateName.Trim(), '_', row.Cell(1).GetString());

                                    var columnAddress = row.Cell(column).Address;

                                    if (columnAddress.ColumnNumber < signNowLinkColumnIndex)
                                        jsonPayload.Append("{").Append('"').Append(columnName).Append('"').Append(":").Append('"').Append(data).Append('"');
                                    if (columnAddress.ColumnNumber < signNowLinkColumnIndex - 1)
                                        jsonPayload.Append("},");


                                    column++;
                                }
                                string jsonSmartFields = jsonPayload.Append("}],").Append('"').Append("client_timestamp").Append('"').Append(":").Append('"').Append("UTC time stamp").Append('"').Append("}").ToString();

                                var SignNowAuthentication = new Authentication();
                                string accessToken = SignNowAuthentication.GenerateToken(_authConfiguration);
                                var auth = JsonSerializer.Deserialize<AuthToken>(accessToken);

                                ISignNowDocumentCreator Document = new SignNowDocumentCreator();

                                string apiUrl = _authConfiguration.SIGNNOW_API_URL;
                                string result = Document.GenerateDocument(auth.access_token, apiUrl, document);

                                var jsonDocument = JsonSerializer.Deserialize<DocumentParameters>(result);
                                document.id = jsonDocument.id;
                                Document.MergeSmartFields(auth.access_token, apiUrl, jsonSmartFields, document);

                                Document.MoveDocument(auth.access_token, apiUrl, document);

                                string link = Document.GenerateLink(auth.access_token, apiUrl, document);
                                var signNowLink = JsonSerializer.Deserialize<DocumentParameters>(link);
                                document.url_no_signup = signNowLink.url_no_signup;
                                //Insert Column
                                row.Cell(signNowLinkColumnIndex).Value = document.url_no_signup;
                                wb.SaveAs(xslxFile);
                                row = row.RowBelow();
                                jsonPayload.Clear();

                                await Task.Delay(10); // It is only to make the process slower

                            }

                        }

                        Console.WriteLine("Document Creation Complete. Documents are ready for signing.");
                        messageAlert = "Document Creation Complete. Documents are ready for signing.";


                    }
                    else
                    {

                    }
                }
                else
                {

                    messageAlert = "A case number merge field is required to be set";

                }
                ViewBag.MessageAlert = messageAlert; ViewBag.MessageExist = "True";
            }
            catch (Exception ex)
            {
                messageAlert = ex.ToString();
            }
            return Json(new { apiResponse = results, message = messageAlert });

        }


        [HttpPost]
        [Route("MassMailer/Progress")]
        public ActionResult Progress()
        {
            return this.Content(Startup.Progress.ToString());
        }

       

    }

    public class DocumentParameters
    {
        public string TemplateId { get; set; }
        public string id { get; set; }

        public string document_name { get; set; }

        public string folder_id { get; set; }
        public string url_no_signup { get; set; }
    }

}


         
       
    


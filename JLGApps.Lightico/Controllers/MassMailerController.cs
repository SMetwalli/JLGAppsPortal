using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using JLGApps.Lightico.Models;
using JLGApps.Lightico.ViewModels;
using JLGApps.Lightico.Controllers.ExcelSheet.Emailer;
using JLGApps.Lightico.Controllers.MessagingService;
using JLGApps.Lightico.Controllers.LighticoApisCalls;
using JLGApps.Lightico;
using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using JLGApps.Lightico.Controllers.LighticoApis;

namespace JLGApps.Lightico.Controllers
{

    public class MassMailerController : Controller
    {
        private LighticoAuthorizationModel _signNowConfiguration { get; set; }
        public MassMailerController(IOptions<LighticoAuthorizationModel> signNowConfiguration)
        {
            _signNowConfiguration = signNowConfiguration.Value;
        }

        [Route("")]
        [Route("MassMailer")]
        [Route("MassMailer/Index")]
        public IActionResult Index()
        {
            var mailboxes = new MailboxSelection();
            var mailboxesList = mailboxes.GetMailboxes();
            ILigticoTemplates templates = new Templates(_signNowConfiguration);
            

            var templateFolders = templates.GetFolders(_signNowConfiguration);
          

            var envelope = new FileLoaderViewModel
            {

                Recipients = null,
                FileAndPath = " ",
                SMSRecipient=" ",
                SMSBody= " ",
                EmailBody = "",
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
                                    string txtMessage,string txtRecipient, string selectedFolder, string folderId,string casenumExcelHeader,string fullNameExcelHeader)
        {
            var mailboxes = new MailboxSelection();
            var mailboxesList = mailboxes.GetMailboxes();
            var RecipientLoader = new ExcelSheetLoader();
            var envelope = new FileLoaderViewModel();
            ILigticoTemplates templates = new Templates(_signNowConfiguration);

            var templateFolders = templates.GetFolders(_signNowConfiguration);
            if (folderId != null && folderId!="NONE")
            {
                var templateProfile = templates.GetTemplates(templateFolders, selectedFolder.Trim(), folderId);

                if (templateProfile != null)
                    envelope.MultipleTemplateProfiles = templateProfile;
            }
          

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

            if (casenumExcelHeader != null)
                envelope.FullNameExcelHeader = fullNameExcelHeader;

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
                                                   string txtRecipient,string selectedFolder,string folderId,string casenumberExcelHeader,string fullNameExcelHeader)
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
            if (!string.IsNullOrEmpty(fullNameExcelHeader))
                fullNameExcelHeader = fullNameExcelHeader.Trim();

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
                casenumExcelHeader = casenumberExcelHeader,
                fullNameExcelHeader = fullNameExcelHeader

            });
        }


        [HttpPost]        
        [Route("MassMailer/LoadData/{xslxFile?}")]
        public ActionResult LoadData(IFormFile xslxFile,string emailBody, string emailSubject, string emailSender, string emailRecipient,string txtMessage,
            string txtRecipient,string folderId,string selectedFolder,string casenumberExcelHeader,string fullNameExcelHeader)
        {
            string excelFile = " ";
            string body = " ";
            string subject = " ";
            string sender = " ";
            string recipient = " ";
            string messageAlert = " ";
            string smsMessage = " ";
            string smsRecipient = " ";

            //string path = @"\\jlgfs01\Mail\Bulk Mailer";

            string path = @"C:\Bulk Mailer";
            if (!string.IsNullOrEmpty(emailRecipient))
                recipient = emailRecipient;


            if (!string.IsNullOrEmpty(emailBody) && emailBody != "<p><br></p>")
            { body = emailBody.Trim(); }
            else
            { messageAlert = ""; }

            if (!string.IsNullOrEmpty(emailSubject))
                subject = emailSubject.Trim();
            if (!string.IsNullOrEmpty(emailSender))
                sender = emailSender.Trim();
           
            if (!string.IsNullOrEmpty(txtMessage))
                smsMessage = txtMessage.Trim();
            if (!string.IsNullOrEmpty(txtRecipient))
                smsRecipient = txtRecipient.Trim();


            if (xslxFile != null)
            {
                try
                {
                    using (var fileStream = new FileStream(Path.Combine(path, xslxFile.FileName), FileMode.Create))
                    {
                        if (Directory.Exists(path) == true)
                            xslxFile.CopyTo(fileStream);
                    }
                    excelFile = Path.Combine(path, xslxFile.FileName);
                 
                }catch(Exception ex)
                {
                  
                    if(ex.HResult== -2147024864)
                        messageAlert = "This excel worksheet cannot be loaded because it is already open and running.\n\r " +
                            "More than one of the same excel worksheet cannot be running at the same time.\n\r  " +
                            "Please close and shut down its process.";

                    messageAlert = ex.ToString();
                }
            }
           

            if (excelFile == " " && TempData["EXCEL_FILE"] != null)
                excelFile = TempData["EXCEL_FILE"].ToString();

            return RedirectToAction("Index", new { excelFile = excelFile, emailBody = body, emailSubject = subject, emailSender = sender, emailRecipient = recipient, messageAlert=messageAlert,
                                                    templateID=" " ,txtMessage= smsMessage,txtRecipient= smsRecipient, selectedFolder= selectedFolder,folderId= folderId,casenumberExcelHeader=casenumberExcelHeader,
                                                    fullNameExcelHeader= fullNameExcelHeader
            });
        }

        
        [HttpPost]      
        [Route("MassMailer/SendEmail/{emailParameters?}")]
        public ActionResult SendEmail([FromBody] SendEmail emailParameters)
         {
            //string recipient = "kwilliams@johnsonlawgroup.com";
            dynamic results = "";
            string excelFile = " ";
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
                    excelFile = emailParameters.xslxFile;
                    var recipientsCollection = RecipientLoader.LoadWorkSheet(excelFile);
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

                            var EmailerHost = new Messaging();

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
                            var EmailerHost = new Messaging();

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
            //Load excel phonenumber and signer link
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
                var recipientsCollection = RecipientLoader.LoadWorkSheet(smsMessage.excelFilePath.Trim());
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

                        var EmailerHost = new Messaging();

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
        [Route("MassMailer/GenerateLighticoDocument/{lighticoTemplateAttributes?}")]
        public async Task<IActionResult> GenerateLighticoDocument([FromBody] LighticoTemplateFields lighticoTemplateAttributes)
        {
            string messageAlert = "";
            string templateName = lighticoTemplateAttributes.templateName;
            string templateId = lighticoTemplateAttributes.templateID;
            string xslxFile = lighticoTemplateAttributes.excelFilePath.Trim();
            string folderId = lighticoTemplateAttributes.templateFolderId;
            dynamic results = "";
            var RecipientLoader = new ExcelSheetLoader();
            var recipientEmail = new EmailParameters();
            StringBuilder jsonSessionPayload = new StringBuilder();
            StringBuilder jsoneSignDocumentPayload = new StringBuilder();


            var document = new DocumentRequestModel
            {
                 TemplateId  = templateId             
            };

           
            try
            {
                if (lighticoTemplateAttributes.caseNumber != "NONE" && !string.IsNullOrEmpty(lighticoTemplateAttributes.templateFolderId))
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
                            int signerLinkColumnIndex = lastPossibleAddress;


                            if (headerRow.Cell(lastPossibleAddress).Value.ToString() != "SIGNER_LINK")
                            {
                                signNowLinkColumn.InsertColumnsAfter(1);
                                signerLinkColumnIndex = lastPossibleAddress + 1;
                                headerRow.Cell(signerLinkColumnIndex).Value = "SIGNER_LINK";

                                wb.SaveAs(xslxFile);
                            }
                            int currentRow = 0;
                            while (!row.Cell(1).IsEmpty())
                            {
                                jsonSessionPayload.Append("{\r\n  ").Append('\"').Append("sourceName").Append('\"').Append(":").Append('\"').Append("lightico").Append('\"').Append(",");
                                jsonSessionPayload.Append("\r\n  ").Append('\"').Append("userName").Append('\"').Append(":").Append('\"').Append("accounts@johnsonlawgroup.com").Append('\"').Append(",");
                                jsonSessionPayload.Append("\r\n  ").Append('\"').Append("customerName").Append('\"').Append(":").Append('\"').Append(lighticoTemplateAttributes.fullName).Append('\"').Append(",");
                                jsonSessionPayload.Append("\r\n  ").Append('\"').Append("email").Append('\"').Append(":").Append('\"').Append("kwilliams@johnsonlawgroup.com").Append('\"').Append(",");
                                jsonSessionPayload.Append("\r\n  ").Append('\"').Append("sendNow").Append('\"').Append(":").Append(" false ").Append(",");
                                jsonSessionPayload.Append("\r\n ").Append('\"').Append("customerData").Append('\"').Append(":{");
                                jsoneSignDocumentPayload.Append("{\r\n  ").Append('\"').Append("name").Append('\"').Append(":").Append('\"').Append("eSignDoc").Append('\"').Append(",");
                                jsoneSignDocumentPayload.Append("\r\n  ").Append('\"').Append("templateId").Append('\"').Append(":").Append('\"').Append(lighticoTemplateAttributes.templateID).Append('\"').Append(",");
                                jsoneSignDocumentPayload.Append("\r\n ").Append('\"').Append("customerData").Append('\"').Append(":{");
                                int column = 1;
                                currentRow++;
                                Startup.Progress = (int)((float)currentRow / (float)totalRecipientCount * 100.0);
                                while (!row.Cell(column).IsEmpty())
                                {

                                    string columnName = headerRow.Cell(column).GetString();
                                    string data = row.Cell(column).GetString();

                                    if (columnName == lighticoTemplateAttributes.caseNumber)
                                    {
                                        string caseNumber = row.Cell(column).GetString();
                                        document.name = string.Concat(templateName.Replace(".pdf"," ").Trim(), '_', caseNumber,".pdf");                                     
                                        jsoneSignDocumentPayload.Replace("eSignDoc", document.name);
                                    }

                                    if (columnName == lighticoTemplateAttributes.fullName)
                                    {
                                        string fullName = row.Cell(column).GetString();                                        
                                        jsonSessionPayload.Replace(lighticoTemplateAttributes.fullName, fullName.Trim());
                                     
                                    }

                                    var columnAddress = row.Cell(column).Address;


                                    if (columnAddress.ColumnNumber < signerLinkColumnIndex)
                                    {
                                        jsonSessionPayload.Append("\r\n  ").Append('\"').Append(columnName).Append('\"').Append(":").Append('\"').Append(data).Append('\"');
                                        jsoneSignDocumentPayload.Append("\r\n  ").Append('\"').Append(columnName).Append('\"').Append(":").Append('\"').Append(data).Append('\"');
                                    }
                                    if (columnAddress.ColumnNumber < signerLinkColumnIndex - 1)
                                    {
                                        jsonSessionPayload.Append(",");
                                        jsoneSignDocumentPayload.Append(",");
                                    }


                                     column++;
                                }


                                string customerSession = jsonSessionPayload.Append("\r\n    }\r\n}").ToString();
                                string customerDocument = jsoneSignDocumentPayload.Append("\r\n    }\r\n}").ToString();


                                var lighticoAuthentication = new Authentication(_signNowConfiguration);
                                var Document = new DocumentCreator(_signNowConfiguration);
                                var authorizationToken = lighticoAuthentication.GenerateToken();
                             
                                var session = Document.CreateNewSession(authorizationToken.access_token, customerSession);


                               var documentResponse= Document.GenerateDocument(authorizationToken.access_token, session.sessionId, customerDocument);

                              
                                ////Insert Column
                                row.Cell(signerLinkColumnIndex).Value = session.customerURL;
                                wb.SaveAs(xslxFile);
                                row = row.RowBelow();
                                jsonSessionPayload.Clear();
                                jsoneSignDocumentPayload.Clear();
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

  
 
   
}


         
       
    


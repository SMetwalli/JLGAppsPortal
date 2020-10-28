using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models
{
    public class EmailTemplateDetail
    {
        [Key]
        public int TemplateId { get; set; }

        [Column(TypeName ="nvarchar(100)")]
        public string TemplateName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Category { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Docket { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string EmailBody { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string EmailRecipient { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string EmailSender { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string EmailSubject { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string SMSBody { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string SMSRecipient { get; set; }
    }
}

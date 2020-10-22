using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.Lightico.Models
{
    public class ViewTemplateDataDetail
    {
        [Key]
        public int DocumentId { get; set; }

        [Column(TypeName ="nvarchar(100)")]
        public string DocumentTitle { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Category { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Docket { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string DateCreated { get; set; }


    }
}

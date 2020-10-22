using Microsoft.EntityFrameworkCore;

namespace JLGProcessPortal.Models
{
    public class EmailTemplateDetailsContext:DbContext
    {
        public EmailTemplateDetailsContext(DbContextOptions<EmailTemplateDetailsContext> options):base(options)
        {

        }

        public DbSet<EmailTemplateDetail> EmailTemplateDetails { get; set; }
    }

   
}

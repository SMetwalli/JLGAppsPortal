using Microsoft.EntityFrameworkCore;

namespace JLGApps.SignNow.Models
{
    public class EmailTemplateDetailsContext:DbContext
    {
        public EmailTemplateDetailsContext(DbContextOptions<EmailTemplateDetailsContext> options):base(options)
        {

        }

        public DbSet<EmailTemplateDetail> EmailTemplateDetails { get; set; }
    }

   
}

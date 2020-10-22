using Microsoft.EntityFrameworkCore;

namespace JLGApps.Lightico.Models
{
    public class ViewTemplateDataDetailsContext:DbContext
    {
        public ViewTemplateDataDetailsContext(DbContextOptions<ViewTemplateDataDetailsContext> options):base(options)
        {

        }

        public DbSet<ViewTemplateDataDetail> EmailTemplateDetails { get; set; }
    }

   
}

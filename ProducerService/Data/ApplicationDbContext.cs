using Microsoft.EntityFrameworkCore;
using ProducerService.Model;

namespace ProducerService.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<SendMessage> SendMessages { get; set; }
    }
}

using ConsumerService.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<ReceiveMessage> ReceiveMessages { get; set; }
    }
}

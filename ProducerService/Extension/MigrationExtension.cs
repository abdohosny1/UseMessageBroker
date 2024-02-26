using Microsoft.EntityFrameworkCore;
using ProducerService.Data;

namespace ProducerService.Extension
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext =
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();

        }
    }
}

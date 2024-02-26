using ConsumerService.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Extension
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

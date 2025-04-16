using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Data
{
    public static class DataExtensions
    {
        public static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BookMyShowDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using WeatherApp.Infrastructure.Persistence;

namespace WeatherApp.Web.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var maxRetryAttempts = 5;
        var delayBetweenRetries = TimeSpan.FromSeconds(5);

        for (int i = 0; i < maxRetryAttempts; i++)
        {
            try
            {
                dbContext.Database.Migrate();
                break;
            }
            catch (Exception ex)
            {
                if (i == maxRetryAttempts - 1) throw;
                Console.WriteLine($"Database connection failed. Retrying in {delayBetweenRetries.TotalSeconds} seconds...");
                Thread.Sleep(delayBetweenRetries);
            }
        }
    }
}
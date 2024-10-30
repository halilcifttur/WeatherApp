using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeatherApp.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.Infrastructure.Configurations;

public static class PostgreSqlConfiguration
{
    public static void AddPostgreSql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}

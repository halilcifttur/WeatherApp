using Microsoft.Extensions.Configuration;
using WeatherApp.Infrastructure.Services;
using WeatherApp.Infrastructure.Settings;
using WeatherApp.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.Infrastructure.Configurations;

public static class ServiceConfiguration
{
    public static void AddServiceConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WeatherApiSettings>(configuration.GetSection("WeatherApiSettings"));
        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<IWeatherBatchService, WeatherBatchService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
    }
}
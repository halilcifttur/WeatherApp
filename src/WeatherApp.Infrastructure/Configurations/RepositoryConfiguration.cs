using WeatherApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Infrastructure.Configurations;

public static class RepositoryConfiguration
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IFavoriteCityRepository, FavoriteCityRepository>();
    }
}
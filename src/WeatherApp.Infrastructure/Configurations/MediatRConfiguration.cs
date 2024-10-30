using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.Infrastructure.Configurations;

public static class MediatRConfiguration
{
    public static void AddMediatR(this IServiceCollection services)
    {
        var assemblies = new[] { Assembly.GetAssembly(typeof(Application.FavoriteCities.Commands.AddFavoriteCity.AddFavoriteCityCommand)) }
                .Where(a => a != null)
                .Cast<Assembly>()
                .ToArray();

        services.AddScoped<IMediator, Mediator>();
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.Infrastructure.Configurations;

public static class RedisConfiguration
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var configOptions = new ConfigurationOptions
        {
            EndPoints = { redisConnectionString },
            AbortOnConnectFail = false
        };
        var multiplexer = ConnectionMultiplexer.Connect(configOptions);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
    }
}

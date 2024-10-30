using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherApp.Domain.Repositories.Interfaces;
using WeatherApp.Domain.Services.Interfaces;
using WeatherApp.Infrastructure.Services;
using WeatherApp.Infrastructure.Settings;

namespace WeatherApp.Test.Services;

public class TestableWeatherService : WeatherService
{
    public TestableWeatherService(
        IRedisCacheService cacheService,
        IOptions<WeatherApiSettings> settings,
        ICityRepository cityRepository,
        ILogger<WeatherService> logger)
        : base(cacheService, settings, cityRepository, logger)
    { }

    public Func<string, Task<double?>> TemperatureFromApi1Mock { get; set; }
    public Func<string, Task<double?>> TemperatureFromApi2Mock { get; set; }

    protected override async Task<double?> GetTemperatureFromApi1(string city)
    {
        return await TemperatureFromApi1Mock(city);
    }

    protected override async Task<double?> GetTemperatureFromApi2(string city)
    {
        return await TemperatureFromApi2Mock(city);
    }
}
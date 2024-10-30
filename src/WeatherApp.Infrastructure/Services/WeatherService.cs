using Flurl.Http;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherApp.Infrastructure.Models;
using WeatherApp.Infrastructure.Settings;
using WeatherApp.Domain.Services.Interfaces;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly WeatherApiSettings _settings;
    private readonly ILogger<WeatherService> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly IRedisCacheService _cacheService;

    public WeatherService(IRedisCacheService cacheService, IOptions<WeatherApiSettings> settings, ICityRepository cityRepository, ILogger<WeatherService> logger)
    {
        _cacheService = cacheService;
        _settings = settings.Value;
        _cityRepository = cityRepository;
        _logger = logger;
    }

    public async Task<double?> GetCachedWeatherAsync(string cityName)
    {
        string cacheKey = $"weather_{cityName.ToLower()}";
        var cachedData = await _cacheService.GetCacheAsync<double?>(cacheKey);
        if (cachedData != null)
        {
            var city = await _cityRepository.GetCityByNameAsync(cityName);
            if (city != null)
            {
                await _cityRepository.UpdateCityTemperatureByName(city.Name, cachedData.Value);
            }
            else
            {
                await _cityRepository.AddCityAsync(new Domain.Entities.City
                {
                    Name = cityName,
                    Temperature = cachedData.Value,
                    LastUpdated = DateTime.UtcNow,
                });
            }
        }
        return await _cacheService.GetCacheAsync<double?>(cacheKey);
    }

    public async Task<double?> GetDatabaseWeatherAsync(string cityName)
    {
        var cityTemperature = await _cityRepository.GetCityTemperatureByName(cityName);
        if (cityTemperature != null)
        {
            await _cacheService.SetCacheAsync($"weather_{cityName.ToLower()}", cityTemperature, TimeSpan.FromMinutes(30));
        }
        return cityTemperature;
    }

    public async Task<double?> GetAverageTemperatureAsync(string cityName)
    {
        var apiResult1 = await GetTemperatureFromApi1(cityName);
        var apiResult2 = await GetTemperatureFromApi2(cityName);

        var averageTemp = (apiResult1, apiResult2) switch
        {
            (double value1, double value2) => (value1 + value2) / 2, // Both have values, calculate average
            (double value1, null) => value1,                         // Only apiResult1 has a value
            (null, double value2) => value2,                         // Only apiResult2 has a value
            _ => (double?)null                                       // Both are null
        };

        if (averageTemp.HasValue)
        {
            string cacheKey = $"weather_{cityName.ToLower()}";
            await _cacheService.SetCacheAsync(cacheKey, averageTemp.Value, TimeSpan.FromMinutes(30));
            var city = await _cityRepository.GetCityByNameAsync(cityName);
            if (city != null) { 
                await _cityRepository.UpdateCityTemperatureByName(cityName, averageTemp.Value);
            }
            else
            {
                await _cityRepository.AddCityAsync(new Domain.Entities.City
                {
                    Name = cityName,
                    Temperature = averageTemp.Value,
                    LastUpdated = DateTime.UtcNow,
                });
            }
        }

        return averageTemp;
    }

    protected virtual async Task<double?> GetTemperatureFromApi1(string city)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await $"{_settings.WeatherApiUrl}?key={_settings.WeatherApiKey}&q={city}&days=1"
                .GetJsonAsync<WeatherApiResponse>();

            stopwatch.Stop();
            _logger.LogInformation("WeatherAPI response time for {City}: {ElapsedMilliseconds}ms", city, stopwatch.ElapsedMilliseconds);

            return response?.current?.temp_c;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to retrieve temperature from WeatherAPI for {City} after {ElapsedMilliseconds}ms", city, stopwatch.ElapsedMilliseconds);
            return null;
        }
    }

    protected virtual async Task<double?> GetTemperatureFromApi2(string city)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await $"{_settings.WeatherStackApiUrl}?access_key={_settings.WeatherStackApiKey}&query={city}"
                .GetJsonAsync<WeatherStackResponse>();

            stopwatch.Stop();
            _logger.LogInformation("WeatherStack response time for {City}: {ElapsedMilliseconds}ms", city, stopwatch.ElapsedMilliseconds);

            return response?.current?.temperature;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to retrieve temperature from WeatherAPI for {City} after {ElapsedMilliseconds}ms", city, stopwatch.ElapsedMilliseconds);
            return null;
        }
    }
}
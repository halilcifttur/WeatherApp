using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using WeatherApp.Domain.Services.Interfaces;

namespace WeatherApp.Infrastructure.Services;

public class WeatherBatchService : IWeatherBatchService
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherBatchService> _logger;
    private readonly ConcurrentDictionary<string, List<TaskCompletionSource<double?>>> _requestQueue = new();

    public WeatherBatchService(IWeatherService weatherService, ILogger<WeatherBatchService> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public async Task<double?> GetWeatherWithBatchingAsync(string city)
    {
        // Check Redis cache first
        string cacheKey = $"weather_{city.ToLower()}";
        var cachedData = await _weatherService.GetCachedWeatherAsync(city);
        if (cachedData != null)
        {
            return cachedData;
        }

        // Check database for data
        var dbData = await _weatherService.GetDatabaseWeatherAsync(city);
        if (dbData != null)
        {
            return dbData;
        }

        _logger.LogInformation("Fetching weather data for city {City}", city);

        var tcs = new TaskCompletionSource<double?>();

        var requestList = _requestQueue.GetOrAdd(city, _ => new List<TaskCompletionSource<double?>>());
        requestList.Add(tcs);

        if (requestList.Count == 1)
        {
            _ = FetchWeatherAfterDelay(city);
        }

        return await tcs.Task;
    }

    private async Task FetchWeatherAfterDelay(string city)
    {
        await Task.Delay(5000);

        if (_requestQueue.TryRemove(city, out var requests))
        {
            double? result = await _weatherService.GetAverageTemperatureAsync(city);

            foreach (var tcs in requests)
            {
                tcs.SetResult(result);
            }
        }
    }
}
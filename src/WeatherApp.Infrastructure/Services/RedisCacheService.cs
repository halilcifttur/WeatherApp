using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using WeatherApp.Domain.Services.Interfaces;

namespace WeatherApp.Infrastructure.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _database = redis.GetDatabase();
        _logger = logger;
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var jsonData = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, jsonData, expiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set cache for key {Key}", key);
            throw;
        }
    }

    public async Task<T> GetCacheAsync<T>(string key)
    {
        try
        {
            var jsonData = await _database.StringGetAsync(key);
            return jsonData.HasValue ? JsonConvert.DeserializeObject<T>(jsonData) : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve cache for key {Key}", key);
            throw;
        }
        
    }
}
namespace WeatherApp.Domain.Services.Interfaces;

public interface IRedisCacheService
{
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
    Task<T> GetCacheAsync<T>(string key);
}
namespace WeatherApp.Domain.Services.Interfaces;

public interface IWeatherService
{
    Task<double?> GetAverageTemperatureAsync(string cityName);
    Task<double?> GetCachedWeatherAsync(string cityName);
    Task<double?> GetDatabaseWeatherAsync(string cityName);
}
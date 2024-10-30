namespace WeatherApp.Domain.Services.Interfaces;

public interface IWeatherBatchService
{
    Task<double?> GetWeatherWithBatchingAsync(string city);
}
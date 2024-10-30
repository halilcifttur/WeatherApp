namespace WeatherApp.Infrastructure.Models;

public class WeatherApiResponse
{
    public CurrentWeather current { get; set; }
}

public class CurrentWeather
{
    public double temp_c { get; set; }
}
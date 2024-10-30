namespace WeatherApp.Infrastructure.Models;

public class WeatherStackResponse
{
    public CurrentWeatherStack current { get; set; }
}

public class CurrentWeatherStack
{
    public double temperature { get; set; }
}
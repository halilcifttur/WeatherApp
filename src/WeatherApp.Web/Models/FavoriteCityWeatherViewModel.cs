namespace WeatherApp.Web.Models;

public class FavoriteCityWeatherViewModel
{
    public Guid FavoriteCityId { get; set; }
    public string CityName { get; set; }
    public double? AverageTemperature { get; set; }
}

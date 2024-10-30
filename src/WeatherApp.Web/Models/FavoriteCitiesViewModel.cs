namespace WeatherApp.Web.Models;

public class FavoriteCitiesViewModel
{
    public List<FavoriteCityWeatherViewModel> FavoriteCities { get; set; }
    public FavoriteCityWeatherViewModel HottestCity { get; set; }
    public FavoriteCityWeatherViewModel ColdestCity { get; set; }
}

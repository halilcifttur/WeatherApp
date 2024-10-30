using MediatR;
using WeatherApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Cities.Queries.GetCityWeather;
using WeatherApp.Application.FavoriteCities.Commands.AddFavoriteCity;
using WeatherApp.Application.FavoriteCities.Queries.GetFavoriteCities;
using WeatherApp.Application.FavoriteCities.Commands.DeleteFavoriteCity;

namespace WeatherApp.Web.Controllers;

public class FavoriteCityController : Controller
{
    private readonly IMediator _mediator;

    public FavoriteCityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Display list of favorite cities
    public async Task<IActionResult> Index()
    {
        var favoriteCities = await _mediator.Send(new GetFavoriteCitiesQuery());
        var favoriteWeatherResults = new List<FavoriteCityWeatherViewModel>();

        double? maxTemp = null;
        double? minTemp = null;
        FavoriteCityWeatherViewModel hottestCity = null;
        FavoriteCityWeatherViewModel coldestCity = null;

        foreach (var favoriteCity in favoriteCities)
        {
            var temperature = await _mediator.Send(new GetCityWeatherQuery(favoriteCity.City.Name));

            var cityWeather = new FavoriteCityWeatherViewModel
            {
                FavoriteCityId = favoriteCity.Id,
                CityName = favoriteCity.City.Name,
                AverageTemperature = temperature
            };

            // Check if this city is the hottest or coldest
            if (temperature.HasValue)
            {
                if (!maxTemp.HasValue || temperature > maxTemp)
                {
                    maxTemp = temperature;
                    hottestCity = cityWeather;
                }

                if (!minTemp.HasValue || temperature < minTemp)
                {
                    minTemp = temperature;
                    coldestCity = cityWeather;
                }
            }

            favoriteWeatherResults.Add(cityWeather);
        }

        var model = new FavoriteCitiesViewModel
        {
            FavoriteCities = favoriteWeatherResults.OrderByDescending(fc => fc.AverageTemperature).ToList(),
            HottestCity = hottestCity,
            ColdestCity = coldestCity
        };

        return View(model);
    }

    // Add a favorite city or cities
    [HttpPost]
    public async Task<IActionResult> AddFavoriteCity(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ViewData["Message"] = "Please enter at least one city name.";
            return View("Index");
        }

        var cityNames = name.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(city => char.ToUpper(city.Trim()[0]) + city.Trim().Substring(1).ToLower())
                          .ToList();

        foreach (var cityName in cityNames)
        {
            await _mediator.Send(new AddFavoriteCityCommand(cityName));
        }

        return RedirectToAction("Index");
    }

    // Remove a favorite city
    [HttpPost]
    public async Task<IActionResult> RemoveFavoriteCity(Guid favoriteCityId)
    {
        await _mediator.Send(new DeleteFavoriteCityCommand(favoriteCityId));
        return RedirectToAction("Index", new {});
    }
}

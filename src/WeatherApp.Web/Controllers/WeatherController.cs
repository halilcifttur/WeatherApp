using MediatR;
using WeatherApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Cities.Queries.GetCityWeather;

namespace WeatherApp.Web.Controllers;

public class WeatherController : Controller
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Get weather for the typed city or cities
    [HttpPost]
    public async Task<IActionResult> GetWeather(string cities)
    {
        if (string.IsNullOrWhiteSpace(cities))
        {
            ViewData["Message"] = "Please enter at least one city.";
            return View("Index");
        }

        var cityNames = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(city => char.ToUpper(city.Trim()[0]) + city.Trim().Substring(1).ToLower())
                              .ToList();

        var weatherResults = new List<WeatherViewModel>();

        foreach (var cityName in cityNames)
        {
            var temperature = await _mediator.Send(new GetCityWeatherQuery(cityName));
            weatherResults.Add(new WeatherViewModel { City = cityName, AverageTemperature = temperature });
        }

        return View("WeatherResult", weatherResults);
    }
}

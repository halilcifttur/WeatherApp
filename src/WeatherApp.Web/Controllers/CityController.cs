using MediatR;
using WeatherApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Cities.Commands.AddCity;
using WeatherApp.Application.Cities.Commands.DeleteCity;
using WeatherApp.Application.Cities.Queries.GetCities;

namespace WeatherApp.Web.Controllers;

public class CityController : Controller
{

    private readonly IMediator _mediator;

    public CityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Display list of cities
    public async Task<IActionResult> Index()
    {
        var cityResults = new List<CityViewModel>();
        var cities = await _mediator.Send(new GetCitiesQuery());
        foreach (var city in cities)
        {
            cityResults.Add(new CityViewModel
            {
                CityId = city.Id,
                CityName = city.Name,
                LastUpdated = city.LastUpdated
            });
        }

        return View(cityResults.OrderByDescending(c => c.LastUpdated).ToList());
    }

    // Add city or cities
    [HttpPost]
    public async Task<IActionResult> AddCity(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ViewData["Message"] = "Please enter at least one city name.";
            var currentCities = await _mediator.Send(new GetCitiesQuery());
            var cityResults = currentCities.Select(city => new CityViewModel
            {
                CityId = city.Id,
                CityName = city.Name
            }).ToList();

            return View("Index", cityResults);
        }

        var cityNames = name.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(city => char.ToUpper(city.Trim()[0]) + city.Trim().Substring(1).ToLower())
                          .ToList();

        foreach (var cityName in cityNames)
        {
            var cityId = await _mediator.Send(new AddCityCommand(cityName));
        }

        return RedirectToAction("Index");
    }

    // Remove a city
    [HttpPost]
    public async Task<IActionResult> RemoveCity(Guid cityId)
    {
        await _mediator.Send(new DeleteCityCommand(cityId));
        return RedirectToAction("Index", new { });
    }
}

using MediatR;

namespace WeatherApp.Application.Cities.Queries.GetCityWeather;

public record GetCityWeatherQuery(string cityName) : IRequest<double?>;
using MediatR;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Cities.Queries.GetCities;

public record GetCitiesQuery() : IRequest<IEnumerable<City>>;
using MediatR;

namespace WeatherApp.Application.Cities.Commands.AddCity;

public record AddCityCommand(string name) : IRequest<Guid>;
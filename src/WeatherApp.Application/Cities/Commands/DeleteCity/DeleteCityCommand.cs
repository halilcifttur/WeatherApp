using MediatR;

namespace WeatherApp.Application.Cities.Commands.DeleteCity;

public record DeleteCityCommand(Guid cityId) : IRequest<Unit>;
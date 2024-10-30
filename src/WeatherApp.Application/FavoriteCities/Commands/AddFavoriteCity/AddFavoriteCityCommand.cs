using MediatR;

namespace WeatherApp.Application.FavoriteCities.Commands.AddFavoriteCity;

public record AddFavoriteCityCommand(string name) : IRequest<Guid>;
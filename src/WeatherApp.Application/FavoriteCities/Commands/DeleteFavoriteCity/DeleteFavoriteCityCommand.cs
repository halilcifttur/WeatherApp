using MediatR;

namespace WeatherApp.Application.FavoriteCities.Commands.DeleteFavoriteCity;

public record DeleteFavoriteCityCommand(Guid favoriteCityId) : IRequest<Unit>;
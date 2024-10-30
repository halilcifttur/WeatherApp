using MediatR;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.FavoriteCities.Queries.GetFavoriteCities;

public record GetFavoriteCitiesQuery() : IRequest<IEnumerable<FavoriteCity>>;
using MediatR;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.FavoriteCities.Queries.GetFavoriteCities;

public class GetFavoriteCitiesQueryHandler : IRequestHandler<GetFavoriteCitiesQuery, IEnumerable<FavoriteCity>>
{
    private readonly IFavoriteCityRepository _favoriteCityRepository;

    public GetFavoriteCitiesQueryHandler(IFavoriteCityRepository favoriteCityRepository)
    {
        _favoriteCityRepository = favoriteCityRepository;
    }

    public async Task<IEnumerable<FavoriteCity>> Handle(GetFavoriteCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _favoriteCityRepository.GetFavoriteCitiesAsync();
    }
}
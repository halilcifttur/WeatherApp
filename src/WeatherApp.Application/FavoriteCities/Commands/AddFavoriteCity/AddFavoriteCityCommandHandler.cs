using MediatR;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Exceptions;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.FavoriteCities.Commands.AddFavoriteCity;

public class AddFavoriteCityCommandHandler : IRequestHandler<AddFavoriteCityCommand, Guid>
{
    private readonly ICityRepository _cityRepository;
    private readonly IFavoriteCityRepository _favoriteCityRepository;

    public AddFavoriteCityCommandHandler(IFavoriteCityRepository favoriteCityRepository, ICityRepository cityRepository)
    {
        _favoriteCityRepository = favoriteCityRepository;
        _cityRepository = cityRepository;
    }

    public async Task<Guid> Handle(AddFavoriteCityCommand request, CancellationToken cancellationToken)
    {

        var city = await _cityRepository.GetCityByNameAsync(request.name);
        if (city == null)
        {
            throw new EntityNotFoundException(nameof(city));
        }

        var favoriteCity = new FavoriteCity
        {
            CityId = city.Id,
        };

        await _favoriteCityRepository.AddFavoriteCityAsync(favoriteCity);
        return favoriteCity.Id;
    }
}
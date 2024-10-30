using MediatR;
using WeatherApp.Domain.Exceptions;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.FavoriteCities.Commands.DeleteFavoriteCity;

public class DeleteFavoriteCityCommandHandler : IRequestHandler<DeleteFavoriteCityCommand, Unit>
{
    private readonly IFavoriteCityRepository _favoriteCityRepository;

    public DeleteFavoriteCityCommandHandler(IFavoriteCityRepository favoriteCityRepository)
    {
        _favoriteCityRepository = favoriteCityRepository;
    }

    public async Task<Unit> Handle(DeleteFavoriteCityCommand request, CancellationToken cancellationToken)
    {
        var favoriteCity = await _favoriteCityRepository.GetFavoriteCityByIdAsync(request.favoriteCityId);
        if (favoriteCity == null)
        {
            throw new EntityNotFoundException(nameof(favoriteCity));
        }

        await _favoriteCityRepository.DeleteFavoriteCityAsync(request.favoriteCityId);
        return Unit.Value;
    }
}
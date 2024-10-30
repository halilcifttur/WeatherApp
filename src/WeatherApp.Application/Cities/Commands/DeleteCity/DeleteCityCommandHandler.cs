using MediatR;
using WeatherApp.Domain.Exceptions;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.Cities.Commands.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Unit>
{
    private readonly ICityRepository _cityRepository;

    public DeleteCityCommandHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = _cityRepository.GetCityByIdAsync(request.cityId);
        if (city == null)
        {
            throw new EntityNotFoundException(nameof(city));
        }

        await _cityRepository.DeleteCityAsync(request.cityId);
        return Unit.Value;
    }
}
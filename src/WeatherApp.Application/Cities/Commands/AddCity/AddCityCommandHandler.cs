using MediatR;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Exceptions;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.Cities.Commands.AddCity;

public class AddCityCommandHandler : IRequestHandler<AddCityCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly ICityRepository _cityRepository;

    public AddCityCommandHandler(IMediator mediator, ICityRepository cityRepository)
    {
        _mediator = mediator;
        _cityRepository = cityRepository;
    }

    public async Task<Guid> Handle(AddCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetCityByNameAsync(request.name);
        if (city != null)
        {
            throw new EntityExistsException(request.name);
        }

        city = new City
        {
            Name = request.name,
            LastUpdated = DateTime.UtcNow,
            Temperature = null
        };

        await _cityRepository.AddCityAsync(city);
        return city.Id;
    }
}
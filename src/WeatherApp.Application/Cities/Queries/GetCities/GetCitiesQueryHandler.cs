using MediatR;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Application.Cities.Queries.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<City>>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<City>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _cityRepository.GetCitiesAsync();
    }
}
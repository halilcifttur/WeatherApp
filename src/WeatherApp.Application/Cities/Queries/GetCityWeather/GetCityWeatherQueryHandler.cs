using MediatR;
using WeatherApp.Domain.Services.Interfaces;

namespace WeatherApp.Application.Cities.Queries.GetCityWeather;

public class GetCityWeatherQueryHandler : IRequestHandler<GetCityWeatherQuery, double?>
{
    private readonly IWeatherBatchService _weatherBatchService;

    public GetCityWeatherQueryHandler(IWeatherBatchService weatherBatchService)
    {
        _weatherBatchService = weatherBatchService;
    }

    public async Task<double?> Handle(GetCityWeatherQuery request, CancellationToken cancellationToken)
    {
        return await _weatherBatchService.GetWeatherWithBatchingAsync(request.cityName);
    }
}
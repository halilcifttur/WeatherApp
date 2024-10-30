using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherApp.Infrastructure.Services;
using WeatherApp.Infrastructure.Settings;
using WeatherApp.Domain.Services.Interfaces;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Test.Services;

public class WeatherServiceTests
{
    private readonly Mock<IRedisCacheService> _mockCacheService;
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IOptions<WeatherApiSettings>> _mockSettings;
    private readonly TestableWeatherService _weatherService;

    public WeatherServiceTests()
    {
        _mockCacheService = new Mock<IRedisCacheService>();
        _mockCityRepository = new Mock<ICityRepository>();
        _mockSettings = new Mock<IOptions<WeatherApiSettings>>();
        _mockSettings.Setup(s => s.Value).Returns(new WeatherApiSettings
        {
            WeatherApiUrl = "http://api.weatherapi.com/v1/forecast.json",
            WeatherApiKey = "147d644004414106a2f75650232001",
            WeatherStackApiUrl = "http://api.weatherstack.com/current",
            WeatherStackApiKey = "838c0d5e8fcc1dbbc66e8c1c0a14c6e5"
        });

        var mockLogger = new Mock<ILogger<WeatherService>>();
        _weatherService = new TestableWeatherService(_mockCacheService.Object, _mockSettings.Object, _mockCityRepository.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetAverageTemperatureAsync_FallsBackToApi2_WhenApi1Fails()
    {
        var cacheMock = new Mock<IRedisCacheService>();
        var cityRepoMock = new Mock<ICityRepository>();
        var settingsMock = new Mock<IOptions<WeatherApiSettings>>();
        settingsMock.Setup(s => s.Value).Returns(new WeatherApiSettings());

        var weatherServiceLogger = new Mock<ILogger<WeatherService>>();

        var service = new TestableWeatherService(
            cacheMock.Object,
            settingsMock.Object,
            cityRepoMock.Object,
            weatherServiceLogger.Object
        );

        service.TemperatureFromApi1Mock = _ => Task.FromResult((double?)null);
        service.TemperatureFromApi2Mock = _ => Task.FromResult((double?)22.5);

        var result = await service.GetAverageTemperatureAsync("Istanbul");

        Assert.Equal(22.5, result);
    }
}
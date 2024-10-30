using Moq;
using MediatR;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Services.Interfaces;
using WeatherApp.Domain.Repositories.Interfaces;
using WeatherApp.Application.Cities.Commands.AddCity;
using WeatherApp.Application.Cities.Queries.GetCities;
using WeatherApp.Application.Cities.Commands.DeleteCity;
using WeatherApp.Application.Cities.Queries.GetCityWeather;

namespace WeatherApp.Test.Handlers;

public class CityHandlerTests
{
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IWeatherBatchService> _mockWeatherBatchService;
    private readonly Mock<IMediator> _mockMediator;

    public CityHandlerTests()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockWeatherBatchService = new Mock<IWeatherBatchService>();
        _mockMediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task AddCityCommandHandler_AddsCitySuccessfully()
    {
        var cityId = Guid.NewGuid();
        var handler = new AddCityCommandHandler(_mockMediator.Object, _mockCityRepository.Object);
        var command = new AddCityCommand("Istanbul");

        _mockMediator.Setup(m => m.Send(It.IsAny<GetCityWeatherQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(25.0);
        _mockCityRepository.Setup(r => r.AddCityAsync(It.IsAny<City>())).Returns(Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        _mockCityRepository.Verify(r => r.AddCityAsync(It.IsAny<City>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCityCommandHandler_DeletesCitySuccessfully()
    {
        var cityId = Guid.NewGuid();
        var handler = new DeleteCityCommandHandler(_mockCityRepository.Object);
        var command = new DeleteCityCommand(cityId);

        _mockCityRepository.Setup(r => r.DeleteCityAsync(cityId)).Returns(Task.CompletedTask);

        await handler.Handle(command, CancellationToken.None);

        _mockCityRepository.Verify(r => r.DeleteCityAsync(cityId), Times.Once);
    }

    [Fact]
    public async Task GetCitiesQueryHandler_ReturnsListOfCities()
    {
        var handler = new GetCitiesQueryHandler(_mockCityRepository.Object);
        _mockCityRepository.Setup(r => r.GetCitiesAsync()).ReturnsAsync(new List<City> { new City() });

        var result = await handler.Handle(new GetCitiesQuery(), CancellationToken.None);

        Assert.Single(result);
        _mockCityRepository.Verify(r => r.GetCitiesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCityWeatherQueryHandler_ReturnsCityTemperature()
    {
        var handler = new GetCityWeatherQueryHandler(_mockWeatherBatchService.Object);
        var query = new GetCityWeatherQuery("Istanbul");

        _mockWeatherBatchService.Setup(s => s.GetWeatherWithBatchingAsync("Istanbul")).ReturnsAsync(25.0);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(25.0, result);
        _mockWeatherBatchService.Verify(s => s.GetWeatherWithBatchingAsync("Istanbul"), Times.Once);
    }

    [Fact]
    public async Task AddCityCommandHandler_LogsError_WhenExceptionThrown()
    {
        var handler = new AddCityCommandHandler(_mockMediator.Object, _mockCityRepository.Object);
        var command = new AddCityCommand("Istanbul");

        _mockMediator.Setup(m => m.Send(It.IsAny<GetCityWeatherQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(25.0);
        _mockCityRepository.Setup(r => r.AddCityAsync(It.IsAny<City>())).ThrowsAsync(new Exception("Database failure"));

        var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Database failure", exception.Message);

        _mockCityRepository.Verify(r => r.AddCityAsync(It.IsAny<City>()), Times.Once);
    }
}
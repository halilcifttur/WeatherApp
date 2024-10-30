using Moq;
using Microsoft.Extensions.Logging;
using WeatherApp.Infrastructure.Services;
using WeatherApp.Domain.Services.Interfaces;

namespace WeatherApp.Test.Services;

public class WeatherBatchServiceTests
{
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly WeatherBatchService _batchService;

    public WeatherBatchServiceTests()
    {
        _mockWeatherService = new Mock<IWeatherService>();
        var mockLogger = new Mock<ILogger<WeatherBatchService>>();
        _batchService = new WeatherBatchService(_mockWeatherService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetWeatherWithBatchingAsync_UsesCacheIfAvailable()
    {
        _mockWeatherService.Setup(s => s.GetCachedWeatherAsync("Istanbul")).ReturnsAsync(25.0);

        var result = await _batchService.GetWeatherWithBatchingAsync("Istanbul");

        Assert.Equal(25.0, result);
        _mockWeatherService.Verify(s => s.GetCachedWeatherAsync("Istanbul"), Times.Once);
        _mockWeatherService.Verify(s => s.GetAverageTemperatureAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetWeatherWithBatchingAsync_UsesDatabaseIfCacheMisses()
    {
        _mockWeatherService.Setup(s => s.GetCachedWeatherAsync("Istanbul")).ReturnsAsync((double?)null);
        _mockWeatherService.Setup(s => s.GetDatabaseWeatherAsync("Istanbul")).ReturnsAsync(22.5);

        var result = await _batchService.GetWeatherWithBatchingAsync("Istanbul");

        Assert.Equal(22.5, result);
        _mockWeatherService.Verify(s => s.GetCachedWeatherAsync("Istanbul"), Times.Once);
        _mockWeatherService.Verify(s => s.GetDatabaseWeatherAsync("Istanbul"), Times.Once);
        _mockWeatherService.Verify(s => s.GetAverageTemperatureAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetWeatherWithBatchingAsync_BatchesRequestsForSameCity()
    {
        _mockWeatherService.Setup(s => s.GetAverageTemperatureAsync("Istanbul")).ReturnsAsync(25.0);

        var task1 = _batchService.GetWeatherWithBatchingAsync("Istanbul");
        var task2 = _batchService.GetWeatherWithBatchingAsync("Istanbul");

        await Task.WhenAll(task1, task2);

        _mockWeatherService.Verify(s => s.GetAverageTemperatureAsync("Istanbul"), Times.Once);
    }
}
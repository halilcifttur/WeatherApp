using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using WeatherApp.Infrastructure.Services;

namespace WeatherApp.Test.Services;

public class RedisCacheServiceTests
{
    private readonly Mock<IDatabase> _mockDatabase;
    private readonly Mock<IConnectionMultiplexer> _mockConnection;
    private readonly RedisCacheService _cacheService;

    public RedisCacheServiceTests()
    {
        _mockDatabase = new Mock<IDatabase>();
        _mockConnection = new Mock<IConnectionMultiplexer>();
        _mockConnection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);
        var mockLogger = new Mock<ILogger<RedisCacheService>>();
        _cacheService = new RedisCacheService(_mockConnection.Object, mockLogger.Object);
    }

    [Fact]
    public async Task SetCacheAsync_SetsValueInCache()
    {
        var key = "testKey";
        var value = "testValue";
        var serializedValue = JsonConvert.SerializeObject(value);

        _mockDatabase.Setup(db => db.StringSetAsync(key, serializedValue, It.IsAny<TimeSpan>(), When.Always, CommandFlags.None))
                     .ReturnsAsync(true);

        await _cacheService.SetCacheAsync(key, value, TimeSpan.FromMinutes(5));

        _mockDatabase.Verify(db => db.StringSetAsync(
            key,
            serializedValue,
            It.IsAny<TimeSpan>(),
            It.IsAny<bool>(), 
            When.Always,
            CommandFlags.None
        ), Times.Once);
    }

    [Fact]
    public async Task GetCacheAsync_ReturnsCachedValue()
    {
        var key = "testKey";
        var cachedValue = JsonConvert.SerializeObject("testValue");
        _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(cachedValue);

        var result = await _cacheService.GetCacheAsync<string>(key);

        Assert.Equal("testValue", result);
    }

    [Fact]
    public async Task GetCacheAsync_ReturnsNull_WhenCacheIsEmpty()
    {
        var key = "testKey";
        _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);

        var result = await _cacheService.GetCacheAsync<string>(key);

        Assert.Null(result);
        _mockDatabase.Verify(db => db.StringGetAsync(key, CommandFlags.None), Times.Once);
    }
}
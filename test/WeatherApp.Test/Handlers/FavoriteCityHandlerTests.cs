using Moq;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Repositories.Interfaces;
using WeatherApp.Application.FavoriteCities.Commands.AddFavoriteCity;
using WeatherApp.Application.FavoriteCities.Queries.GetFavoriteCities;
using WeatherApp.Application.FavoriteCities.Commands.DeleteFavoriteCity;
using WeatherApp.Domain.Exceptions;

namespace WeatherApp.Test.Handlers;

public class FavoriteCityHandlerTests
{
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IFavoriteCityRepository> _mockFavoriteCityRepository;

    public FavoriteCityHandlerTests()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockFavoriteCityRepository = new Mock<IFavoriteCityRepository>();
    }

    [Fact]
    public async Task AddFavoriteCityCommandHandler_AddsCitySuccessfully()
    {
        var cityId = Guid.NewGuid();
        var handler = new AddFavoriteCityCommandHandler(_mockFavoriteCityRepository.Object, _mockCityRepository.Object);
        var command = new AddFavoriteCityCommand("Kayseri");

        _mockCityRepository.Setup(r => r.GetCityByNameAsync("Kayseri")).ReturnsAsync(new City { Id = cityId });
        _mockFavoriteCityRepository.Setup(r => r.AddFavoriteCityAsync(It.IsAny<FavoriteCity>())).Returns(Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        _mockFavoriteCityRepository.Verify(r => r.AddFavoriteCityAsync(It.IsAny<FavoriteCity>()), Times.Once);
        _mockCityRepository.Verify(r => r.GetCityByNameAsync("Kayseri"), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteCityCommandHandler_ThrowsEntityNotFoundException_WhenCityNotFound()
    {
        var handler = new AddFavoriteCityCommandHandler(_mockFavoriteCityRepository.Object, _mockCityRepository.Object);
        var command = new AddFavoriteCityCommand("UnknownCity");

        _mockCityRepository.Setup(r => r.GetCityByNameAsync("UnknownCity")).ReturnsAsync((City)null);

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("The entity \"city\" was not found.", exception.Message);

        _mockCityRepository.Verify(r => r.GetCityByNameAsync("UnknownCity"), Times.Once);
    }

    [Fact]
    public async Task DeleteFavoriteCityCommandHandler_DeletesCitySuccessfully()
    {
        var favoriteCityId = Guid.NewGuid();
        var handler = new DeleteFavoriteCityCommandHandler(_mockFavoriteCityRepository.Object);
        var command = new DeleteFavoriteCityCommand(favoriteCityId);

        _mockFavoriteCityRepository.Setup(r => r.GetFavoriteCityByIdAsync(favoriteCityId)).ReturnsAsync(new FavoriteCity { Id = favoriteCityId });
        _mockFavoriteCityRepository.Setup(r => r.DeleteFavoriteCityAsync(favoriteCityId)).Returns(Task.CompletedTask);

        await handler.Handle(command, CancellationToken.None);

        _mockFavoriteCityRepository.Verify(r => r.DeleteFavoriteCityAsync(favoriteCityId), Times.Once);
    }

    [Fact]
    public async Task GetFavoriteCitiesQueryHandler_ReturnsFavoriteCities()
    {
        var handler = new GetFavoriteCitiesQueryHandler(_mockFavoriteCityRepository.Object);
        _mockFavoriteCityRepository.Setup(r => r.GetFavoriteCitiesAsync()).ReturnsAsync(new List<FavoriteCity> { new FavoriteCity() });

        var result = await handler.Handle(new GetFavoriteCitiesQuery(), CancellationToken.None);

        Assert.Single(result);
        _mockFavoriteCityRepository.Verify(r => r.GetFavoriteCitiesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetFavoriteCityByName_ThrowsEntityNotFoundException_WhenCityNotFound()
    {
        var handler = new AddFavoriteCityCommandHandler(_mockFavoriteCityRepository.Object, _mockCityRepository.Object);
        var command = new AddFavoriteCityCommand("UnknownCity");

        _mockCityRepository.Setup(r => r.GetCityByNameAsync("UnknownCity")).ReturnsAsync((City)null);

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("The entity \"city\" was not found.", exception.Message);

        _mockCityRepository.Verify(r => r.GetCityByNameAsync("UnknownCity"), Times.Once);
    }
}
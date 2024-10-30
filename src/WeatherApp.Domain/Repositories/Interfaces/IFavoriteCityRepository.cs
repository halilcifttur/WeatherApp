using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Repositories.Interfaces;

public interface IFavoriteCityRepository
{
    Task<FavoriteCity> GetFavoriteCityByIdAsync(Guid id);
    Task<FavoriteCity> GetFavoriteCityByNameAsync(string name);
    Task<IEnumerable<FavoriteCity>> GetFavoriteCitiesAsync();
    Task AddFavoriteCityAsync(FavoriteCity favoriteCity);
    Task DeleteFavoriteCityAsync(Guid id);
}
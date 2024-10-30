using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Repositories.Interfaces;

public interface ICityRepository
{
    Task<City> GetCityByIdAsync(Guid id);
    Task<City> GetCityByNameAsync(string name);
    Task<IEnumerable<City>> GetCitiesAsync();
    Task AddCityAsync(City city);
    Task UpdateCityAsync(City city);
    Task DeleteCityAsync(Guid id);
    Task<double?> GetCityTemperatureByName(string name);
    Task UpdateCityTemperatureByName(string name, double newTemp);
}
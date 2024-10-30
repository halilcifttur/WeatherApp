using WeatherApp.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Infrastructure.Persistence;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CityRepository> _logger;

    public CityRepository(AppDbContext context, ILogger<CityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<City> GetCityByIdAsync(Guid id)
    {
        try
        {
            var city = await _context.Cities.FindAsync(id);
            return city;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve city with ID {id}", id);
            throw;
        }
    }

    public async Task<City> GetCityByNameAsync(string name)
    {
        try
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
            return city;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve city with Name {name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        try
        {
            var cities = await _context.Cities.ToListAsync();
            return cities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve cities");
            throw;
        }
    }

    public async Task AddCityAsync(City city)
    {
        try
        {
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User added city with id: {cityId}", city.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add city {CityName}", city.Name);
            throw;
        }        
    }

    public async Task UpdateCityAsync(City city)
    {
        try
        {
            var existingCity = await GetCityByIdAsync(city.Id);

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update city {CityName}", city.Name);
            throw;
        }
    }

    public async Task DeleteCityAsync(Guid id)
    {
        try
        {
            var city = await GetCityByIdAsync(id);

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete city with id {id}", id);
            throw;
        }
    }

    public async Task<double?> GetCityTemperatureByName(string name)
    {
        try
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
            if (city == null)
            {
                return null;
            }

            return city.Temperature;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to to retrieve the temperature for the city {name}", name);
            throw;
        }
    }

    public async Task UpdateCityTemperatureByName(string name, double newTemp)
    {
        try
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
            if (city != null)
            {
                city.Temperature = newTemp;
                _context.Cities.Update(city);
                await _context.SaveChangesAsync();
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to to update the temperature to {newTemp} for the city {name}", newTemp, name);
            throw;
        }
        
    }
}
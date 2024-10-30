using WeatherApp.Domain.Entities;
using Microsoft.Extensions.Logging;
using WeatherApp.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Infrastructure.Persistence;
using WeatherApp.Domain.Repositories.Interfaces;

namespace WeatherApp.Infrastructure.Repositories;

public class FavoriteCityRepository : IFavoriteCityRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<FavoriteCityRepository> _logger;

    public FavoriteCityRepository(AppDbContext context, ILogger<FavoriteCityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<FavoriteCity> GetFavoriteCityByIdAsync(Guid id)
    {
        try
        {
            var favoriteCity = await _context.FavoriteCities.FindAsync(id);
            if (favoriteCity == null)
            {
                throw new EntityNotFoundException(nameof(favoriteCity));
            }
            return favoriteCity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve favorite city with ID {id}", id);
            throw;
        }
    }

    public async Task<FavoriteCity> GetFavoriteCityByNameAsync(string name)
    {
        try
        {
            var city = await _context.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
            if (city == null)
            {
                throw new EntityNotFoundException(nameof(city));
            }

            var favoriteCity = await _context.FavoriteCities.Include(fc => fc.City).FirstOrDefaultAsync(fc => fc.CityId == city.Id);
            if (favoriteCity == null)
            {
                throw new EntityNotFoundException(nameof(favoriteCity));
            }

            return favoriteCity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve favorite city with Name {name}", name);
            throw;
        }        
    }

    public async Task<IEnumerable<FavoriteCity>> GetFavoriteCitiesAsync()
    {
        try
        {
            var favoriteCities = await _context.FavoriteCities.Include(fc => fc.City).ToListAsync();
            if (favoriteCities == null)
            {
                throw new EntityNotFoundException(nameof(favoriteCities));
            }
            return favoriteCities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve favorite cities");
            throw;
        }
    }

    public async Task AddFavoriteCityAsync(FavoriteCity favoriteCity)
    {
        try
        {
            await _context.FavoriteCities.AddAsync(favoriteCity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User added favorite city with id: {cityId}", favoriteCity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add favorite city");
            throw;
        }
    }

    public async Task DeleteFavoriteCityAsync(Guid id)
    {
        try
        {
            var favoriteCity = await GetFavoriteCityByIdAsync(id);
            if (favoriteCity == null)
            {
                throw new EntityNotFoundException(nameof(favoriteCity));
            }

            _context.FavoriteCities.Remove(favoriteCity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete favorite city with ID {id}", id);
            throw;
        }
    }
}
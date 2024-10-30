using WeatherApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<City> Cities { get; set; }
    public DbSet<FavoriteCity> FavoriteCities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteCity>()
            .HasOne(fc => fc.City)
            .WithMany()
            .HasForeignKey(fc => fc.CityId);

        base.OnModelCreating(modelBuilder);
    }
}
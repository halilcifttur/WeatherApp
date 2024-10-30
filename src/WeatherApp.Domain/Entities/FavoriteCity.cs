namespace WeatherApp.Domain.Entities;

public class FavoriteCity
{
    public FavoriteCity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public virtual City? City { get; set; }
}
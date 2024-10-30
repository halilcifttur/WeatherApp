namespace WeatherApp.Domain.Entities;

public class City
{
    public City()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public double? Temperature { get; set; }
    public DateTime LastUpdated { get; set; }
}
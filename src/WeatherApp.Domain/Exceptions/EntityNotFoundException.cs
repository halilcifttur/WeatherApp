namespace WeatherApp.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName)
            : base($"The entity \"{entityName}\" was not found.")
    {
    }
}
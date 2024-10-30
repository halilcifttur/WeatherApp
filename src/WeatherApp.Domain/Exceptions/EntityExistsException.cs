namespace WeatherApp.Domain.Exceptions;

public class EntityExistsException : Exception
{
    public EntityExistsException(string entityName)
        : base($"The entity \"{entityName}\" is already exists.")
    {
    }
}
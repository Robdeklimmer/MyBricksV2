namespace MyBricks.Domain.Exceptions;

/// <summary>
/// Thrown when a requested entity does not exist in the database.
/// Maps to HTTP 404 in the global exception handler.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.") { }
}

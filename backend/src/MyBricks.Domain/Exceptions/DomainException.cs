namespace MyBricks.Domain.Exceptions;

/// <summary>
/// Thrown when a business rule is violated (e.g., user already in a group).
/// Maps to HTTP 409 Conflict in the global exception handler.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

namespace OrderManagement.Shared.Exceptions;
public abstract class DomainError(string key, string message)
{
    public string Key { get; } = key;

    public string Message { get; } = message;
}
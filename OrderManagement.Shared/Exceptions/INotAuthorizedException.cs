namespace OrderManagement.Shared.Exceptions;

public interface INotAuthorizedException { }

public abstract class NotAuthorizedException<T> : DomainException<T>, INotAuthorizedException
    where T : DomainError
{
    protected NotAuthorizedException()
        : base()
    {
    }
}
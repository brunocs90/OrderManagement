namespace OrderManagement.Shared.Exceptions;

public interface INotFoundException { }

public abstract class NotFoundException<T> : DomainException<T>, INotFoundException
    where T : DomainError
{
    protected NotFoundException()
        : base()
    {
    }
}
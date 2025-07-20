namespace OrderManagement.Shared.Exceptions;

public abstract class DomainException(string message) : Exception(message)
{
    protected ICollection<DomainError> errors = new List<DomainError>();

    public IEnumerable<DomainError> Errors => errors;

    public abstract string Key { get; }
}

public abstract class DomainException<T> : DomainException
    where T : DomainError
{
    protected DomainException()
        : base("Ocorreu um erro de negócio, verifique a propriedade" +
                " 'errors' para obter detalhes.")
    {
    }

    protected DomainException(string message)
        : base(message)
    {
    }

    public DomainException AddError(params T[] errors)
    {
        foreach (var error in errors)
            this.errors.Add(error);

        return this;
    }
}
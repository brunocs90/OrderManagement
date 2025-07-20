using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Domain.Pedidos.Exceptions;

public class CreateOrderException : DomainException<CreateOrderError>
{
    public override string Key => nameof(CreateOrderException);

    public CreateOrderException(CreateOrderError error) : base()
        => AddError(error);
}

public sealed class CreateOrderError : DomainError
{
    public static CreateOrderError UnformedData
        => new(nameof(UnformedData), "Os dados do pedido a serem adicionados devem ser informados.");

    public static CreateOrderError DuplicateExternalOrderId
        => new(nameof(DuplicateExternalOrderId), "Já existe um pedido com o ExternalOrderId informado.");

    public static CreateOrderError InvalidItems
        => new(nameof(InvalidItems), "O pedido deve conter pelo menos um item.");

    private CreateOrderError(string code, string message) : base(code, message)
    { }
}
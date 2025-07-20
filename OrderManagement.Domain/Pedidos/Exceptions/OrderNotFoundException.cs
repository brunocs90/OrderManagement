using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Domain.Pedidos.Exceptions;

public class OrderNotFoundException : DomainException<OrderError>
{
    public override string Key => nameof(OrderNotFoundException);

    public OrderNotFoundException() : base()
        => AddError(OrderError.NotFound);
}

public sealed class OrderError : DomainError
{
    public static OrderError NotFound
        => new(nameof(NotFound), "Pedido não encontrado.");

    public static OrderError DuplicateOrder
        => new(nameof(DuplicateOrder), "Pedido duplicado.");

    public static OrderError InvalidStatus
        => new(nameof(InvalidStatus), "Status inválido.");

    public static OrderError CalculationError
        => new(nameof(CalculationError), "Erro no cálculo do pedido.");

    private OrderError(string code, string message) : base(code, message)
    { }
}
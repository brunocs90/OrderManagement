using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Domain.Pedidos.Exceptions;

public class UpdateOrderException : DomainException<UpdateOrderError>
{
    public override string Key => nameof(UpdateOrderException);

    public UpdateOrderException(UpdateOrderError error) : base()
        => AddError(error);
}

public sealed class UpdateOrderError : DomainError
{
    public static UpdateOrderError UnformedData
        => new(nameof(UnformedData), "Os dados do pedido a serem atualizados devem ser informados.");

    public static UpdateOrderError InvalidStatus
        => new(nameof(InvalidStatus), "Status inválido para atualização.");

    public static UpdateOrderError CalculationError
        => new(nameof(CalculationError), "Erro no cálculo do pedido.");

    private UpdateOrderError(string code, string message) : base(code, message)
    { }
}
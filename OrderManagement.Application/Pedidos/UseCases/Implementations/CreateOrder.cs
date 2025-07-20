using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Shared;

namespace OrderManagement.Application.Pedidos.UseCases.Implementations;

internal class CreateOrder(IOrderDbAdapter orderDbAdapter) : ICreateOrder
{
    private readonly IOrderDbAdapter orderDbAdapter = orderDbAdapter
        ?? throw new ArgumentNullException(nameof(orderDbAdapter));

    public async Task<CreateOrderOutput> Execute(CreateOrderInput input)
    {
        if (input is null)
            throw new CreateOrderException(CreateOrderError.UnformedData);

        ValidationHelper.Validate(input);

        if (input.Items.Count == 0)
            throw new CreateOrderException(CreateOrderError.InvalidItems);

        var exists = await orderDbAdapter.ExistsByExternalOrderId(input.ExternalOrderId);
        if (exists)
            throw new CreateOrderException(CreateOrderError.DuplicateExternalOrderId);

        var order = input.ToModel();
        await orderDbAdapter.Save(order);

        return new CreateOrderOutput(order);
    }
}
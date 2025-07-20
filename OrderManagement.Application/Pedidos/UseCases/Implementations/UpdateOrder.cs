using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Shared;

namespace OrderManagement.Application.Pedidos.UseCases.Implementations;

internal class UpdateOrder(IOrderDbAdapter orderDbAdapter) : IUpdateOrder
{
    private readonly IOrderDbAdapter orderDbAdapter = orderDbAdapter
        ?? throw new ArgumentNullException(nameof(orderDbAdapter));

    public async Task<UpdateOrderOutput> Execute(Guid id, UpdateOrderInput input)
    {
        if (input is null)
            throw new UpdateOrderException(UpdateOrderError.UnformedData);

        ValidationHelper.Validate(input);

        var order = await orderDbAdapter.GetById(id);
        if (order is null)
            throw new OrderNotFoundException();

        input.UpdateModel(order);
        await orderDbAdapter.Save(order);

        return new UpdateOrderOutput(order);
    }
}
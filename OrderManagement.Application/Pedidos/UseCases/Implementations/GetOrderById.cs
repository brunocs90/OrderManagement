using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Exceptions;

namespace OrderManagement.Application.Pedidos.UseCases.Implementations;

internal class GetOrderById(IOrderDbAdapter orderDbAdapter) : IGetOrderById
{
    private readonly IOrderDbAdapter orderDbAdapter = orderDbAdapter
        ?? throw new ArgumentNullException(nameof(orderDbAdapter));

    public async Task<GetOrderByIdOutput> Execute(Guid id)
    {
        var order = await orderDbAdapter.GetById(id);

        if (order is null)
            throw new OrderNotFoundException();

        return new GetOrderByIdOutput(order);
    }
}
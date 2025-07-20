using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Filters;

namespace OrderManagement.Application.Pedidos.UseCases.Implementations;

internal class GetOrders(IOrderDbAdapter orderDbAdapter) : IGetOrders
{
    private readonly IOrderDbAdapter orderDbAdapter = orderDbAdapter
        ?? throw new ArgumentNullException(nameof(orderDbAdapter));

    public async Task<PagedListDto<GetOrdersOutput>> Execute(GetOrdersInput input)
    {
        var filter = new OrderFilter
        {
            OrderNumber = input.OrderNumber,
            ExternalOrderId = input.ExternalOrderId,
            Status = input.Status,
            DataCriacaoInicio = input.DataCriacaoInicio,
            DataCriacaoFim = input.DataCriacaoFim,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };

        var result = await orderDbAdapter.Get(filter);
        var output = new PagedListDto<GetOrdersOutput>
        {
            Items = result.Items.Select(order => new GetOrdersOutput(order)).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            PageCount = result.PageCount
        };

        return output;
    }
}
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IGetOrders
{
    Task<PagedListDto<GetOrdersOutput>> Execute(GetOrdersInput input);
}
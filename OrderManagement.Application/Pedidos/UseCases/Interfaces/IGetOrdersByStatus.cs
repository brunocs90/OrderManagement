using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IGetOrdersByStatus
{
    Task<PagedListDto<GetOrdersOutput>> Execute(GetOrdersByStatusInput input);
}
using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IGetOrderById
{
    Task<GetOrderByIdOutput> Execute(Guid id);
}
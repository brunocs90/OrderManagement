using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface ICreateOrder
{
    Task<CreateOrderOutput> Execute(CreateOrderInput input);
}
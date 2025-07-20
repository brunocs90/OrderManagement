using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IUpdateOrder
{
    Task<UpdateOrderOutput> Execute(Guid id, UpdateOrderInput input);
}
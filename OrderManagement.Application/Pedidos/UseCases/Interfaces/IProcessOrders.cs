using OrderManagement.Application.Pedidos.UseCases.Dtos;

namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IProcessOrders
{
    Task<ProcessOrdersOutput> Execute(ProcessOrdersInput input);
}
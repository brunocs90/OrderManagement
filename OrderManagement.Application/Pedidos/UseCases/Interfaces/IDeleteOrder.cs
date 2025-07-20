namespace OrderManagement.Application.Pedidos.UseCases.Interfaces;

public interface IDeleteOrder
{
    Task Execute(Guid id);
}
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Domain.Pedidos.Filters;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.Adapters;

public interface IOrderDbAdapter
{
    Task<PagedListDto<Order>> Get(OrderFilter filter);
    Task<Order?> GetById(Guid id);
    Task<Order?> GetByExternalOrderId(string externalOrderId);
    Task Save(Order order);
    Task Delete(Order order);
    Task<bool> ExistsByExternalOrderId(string externalOrderId);
    Task<List<Order>> GetByIds(List<Guid> ids);
}
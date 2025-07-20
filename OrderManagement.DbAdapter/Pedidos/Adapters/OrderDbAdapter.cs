using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.DbAdapter.Context;
using OrderManagement.DbAdapter.Extensions;
using OrderManagement.Domain.Pedidos.Filters;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.DbAdapter.Pedidos.Adapters;

public class OrderDbAdapter(OrderManagementContext context) : IOrderDbAdapter
{
    private readonly OrderManagementContext context = context
        ?? throw new ArgumentNullException(nameof(context));

    public async Task<PagedListDto<Order>> Get(OrderFilter filter)
    {
        var query = context.Orders
            .Include(o => o.Items)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.OrderNumber))
            query = query.Where(o => o.OrderNumber.Contains(filter.OrderNumber));

        if (!string.IsNullOrEmpty(filter.ExternalOrderId))
            query = query.Where(o => o.ExternalOrderId.Contains(filter.ExternalOrderId));

        if (filter.Status.HasValue)
            query = query.Where(o => o.Status == filter.Status.Value);

        if (filter.DataCriacaoInicio.HasValue)
            query = query.Where(o => o.CreatedAt >= filter.DataCriacaoInicio.Value);

        if (filter.DataCriacaoFim.HasValue)
            query = query.Where(o => o.CreatedAt <= filter.DataCriacaoFim.Value);

        var result = await query.OrderByDescending(x => x.CreatedAt).GetPaged(filter.PageNumber, filter.PageSize);

        return result;
    }

    public async Task<Order?> GetById(Guid id)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByExternalOrderId(string externalOrderId)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.ExternalOrderId == externalOrderId);
    }

    public async Task Save(Order order)
    {
        if (order.Id == Guid.Empty)
        {
            context.Orders.Add(order);
        }
        else
        {
            context.Orders.Update(order);
        }

        await context.SaveChangesAsync();
    }

    public async Task Delete(Order order)
    {
        order.DeletedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByExternalOrderId(string externalOrderId)
    {
        return await context.Orders.AnyAsync(o => o.ExternalOrderId == externalOrderId);
    }

    public async Task<List<Order>> GetByIds(List<Guid> ids)
    {
        return await context.Orders
            .Include(o => o.Items)
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Security.Adapters;
using OrderManagement.DbAdapter.Context;
using OrderManagement.Domain.Security.Filters;
using OrderManagement.Domain.Security.Models;

namespace OrderManagement.DbAdapter.Security.Adapters;

public class UserDbAdapter(OrderManagementContext context) : IUserDbAdapter
{
    private readonly OrderManagementContext context = context
        ?? throw new ArgumentNullException(nameof(context));

    public async Task<PagedListDto<User>> Get(UserFilter filter)
    {
        var query = context.Users.AsQueryable();

        if (filter.Id is not null)
            query = query.Where(x => x.Id == filter.Id);

        if (!string.IsNullOrWhiteSpace(filter.Login))
            query = query.Where(x => x.Login == filter.Login);

        if (!string.IsNullOrWhiteSpace(filter.Senha))
            query = query.Where(x => x.Password == filter.Senha);

        if (filter.Active is not null)
            query = query.Where(x => x.Active == filter.Active);

        if (filter.AsNoTracking)
            query = query.AsNoTracking();

        var total = await query.CountAsync();
        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var items = await query
            .OrderBy(x => x.Login)
            .Skip(skip)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedListDto<User>
        {
            Items = items,
            TotalCount = total,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }
}
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Commons.Dtos;

namespace OrderManagement.DbAdapter.Extensions;

public static class PaginationExtension
{
    public static async Task<PagedListDto<T>> GetPaged<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
    {
        var result = new PagedListDto<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = await query.CountAsync()
        };

        var pageCount = (double)result.TotalCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (pageNumber - 1) * pageSize;
        result.Items = await query.Skip(skip).Take(pageSize).ToListAsync();

        return result;
    }
}
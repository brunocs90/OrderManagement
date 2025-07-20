using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Domain.Security.Filters;
using OrderManagement.Domain.Security.Models;

namespace OrderManagement.Application.Security.Adapters;

public interface IUserDbAdapter
{
    Task<PagedListDto<User>> Get(UserFilter filter);
}
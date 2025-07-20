using OrderManagement.Domain.Security.Models;

namespace OrderManagement.Application.Security.Adapters;

public interface IAuthenticatedUserAdapter
{
    AuthenticatedUser? GetAuthenticatedUser();
}
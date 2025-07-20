using OrderManagement.Application.Security.UseCases.Dtos;

namespace OrderManagement.Application.Security.UseCases.Interfaces;

public interface IAuthenticateUserPortal
{
    Task<AuthenticateUserOutput> Execute(AuthenticateUserPortalInput input);
}
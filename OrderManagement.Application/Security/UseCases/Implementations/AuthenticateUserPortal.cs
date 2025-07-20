using OrderManagement.Application.Security.Adapters;
using OrderManagement.Application.Security.UseCases.Dtos;
using OrderManagement.Application.Security.UseCases.Interfaces;
using OrderManagement.Domain.Security.Exceptions;
using OrderManagement.Domain.Security.Filters;
using OrderManagement.Shared;
using OrderManagement.Shared.Extensions;

namespace OrderManagement.Application.Security.UseCases.Implementations;

public class AuthenticateUserPortal(IUserDbAdapter userDbAdapter) : IAuthenticateUserPortal
{
    private readonly IUserDbAdapter userDbAdapter = userDbAdapter
        ?? throw new ArgumentNullException(nameof(userDbAdapter));

    public async Task<AuthenticateUserOutput> Execute(AuthenticateUserPortalInput input)
    {
        if (input is null)
            throw new AuthenticateUserPortalException(AuthenticateUserPortalError.UnformedData);

        ValidationHelper.Validate(input);

        var senha = input.Password.Encrypt();
        var usuariosPaged = await userDbAdapter.Get(new UserFilter { Login = input.Username, Senha = senha, Active = true });
        var usuario = usuariosPaged.Items.FirstOrDefault()
            ?? throw new AuthenticateUserPortalException(AuthenticateUserPortalError.NotFound);

        var result = new AuthenticateUserOutput(usuario);
        return result;
    }
}
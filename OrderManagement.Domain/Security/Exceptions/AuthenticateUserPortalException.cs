using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Domain.Security.Exceptions;

public class AuthenticateUserPortalException : DomainException<AuthenticateUserPortalError>
{
    public override string Key => nameof(AuthenticateUserPortalException);

    public AuthenticateUserPortalException(AuthenticateUserPortalError error)
        => AddError(error);
}

public sealed class AuthenticateUserPortalError : DomainError
{
    public static AuthenticateUserPortalError UnformedData
        => new(nameof(UnformedData), "Access data must be provided.");

    public static AuthenticateUserPortalError NotFound
        => new(nameof(NotFound), "Invalid username and/or password.");

    private AuthenticateUserPortalError(string code, string message) : base(code, message)
    { }
}
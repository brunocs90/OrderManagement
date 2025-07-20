using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Domain.Security.Exceptions;
public class UserNotAuthorizedException : NotFoundException<UserNotAuthorizedError>
{
    public override string Key => nameof(UserNotAuthorizedException);
    public UserNotAuthorizedException(UserNotAuthorizedError error)
        => AddError(error);
}
public sealed class UserNotAuthorizedError : DomainError
{
    public static UserNotAuthorizedError ByToken
        => new(nameof(ByToken), "Unauthorized access");

    private UserNotAuthorizedError(string code, string message) : base(code, message)
    { }
}
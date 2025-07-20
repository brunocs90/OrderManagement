using OrderManagement.Application.Security.Adapters;
using OrderManagement.Domain.Security.Models;

namespace OrderManagement.Api.Adapters;

public class AuthenticatedUserAdapter(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserAdapter
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor
        ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public AuthenticatedUser? GetAuthenticatedUser()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return null;

        var userIdClaim = httpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return null;

        var loginClaim = httpContext.User.FindFirst("login")?.Value;
        var nameClaim = httpContext.User.FindFirst("name")?.Value;
        var emailClaim = httpContext.User.FindFirst("email")?.Value;

        return new AuthenticatedUser
        {
            Id = userId,
            Login = loginClaim ?? string.Empty,
            Name = nameClaim ?? string.Empty,
            Email = emailClaim ?? string.Empty
        };
    }
}
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Security.UseCases.Dtos;
using OrderManagement.Application.Security.UseCases.Interfaces;
using OrderManagement.Api.Utils;
using OrderManagement.Api.Controllers;

namespace OrderManagement.Api.Controllers.Security;

[ApiController]
[Route("api/v{version:apiVersion}/authentication")]
public class AuthenticationController(IAuthenticateUserPortal authenticateUserPortal) : ApiControllerBase
{
    private readonly IAuthenticateUserPortal authenticateUserPortal = authenticateUserPortal
            ?? throw new ArgumentNullException(nameof(authenticateUserPortal));

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateUserOutput))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login(AuthenticateUserPortalInput input)
    {
        var usuarioAutenticado = await authenticateUserPortal.Execute(input);
        usuarioAutenticado.DataExpiracao = DateTimeOffset.UtcNow.AddHours(2);
        usuarioAutenticado.Token = JwtGenerator.CreateToken(
            usuarioAutenticado.Usuario!.Id,
            usuarioAutenticado.DataExpiracao!.Value
        );

        return Ok(usuarioAutenticado);
    }
}
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagement.Api.Utils;

public static class JwtGenerator
{
    public static string CreateToken(Guid usuarioId, DateTimeOffset dataExpiracao)
    {
        var claims = new[]
        {
            new Claim(JwtInfo.USER_ID, usuarioId.ToString()),
            new Claim(JwtInfo.EXPIRATION, dataExpiracao.ToUnixTimeSeconds().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtInfo.SECRET_KEY));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            JwtInfo.ISSUER,
            JwtInfo.AUDIENCE,
            claims,
            expires: dataExpiracao.UtcDateTime,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
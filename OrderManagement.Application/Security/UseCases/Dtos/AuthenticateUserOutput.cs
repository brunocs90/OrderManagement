using System.ComponentModel.DataAnnotations;
using OrderManagement.Domain.Security.Models;

namespace OrderManagement.Application.Security.UseCases.Dtos;

public class AuthenticateUserOutput
{
    public Guid Id { get; set; }
    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }
    public DateTimeOffset? DataExpiracao { get; set; }
    public AuthenticatedUser? Usuario { get; set; }

    public AuthenticateUserOutput(User usuario)
    {
        Id = usuario.Id;
        Username = usuario.Login;
        Usuario = new AuthenticatedUser
        {
            Id = usuario.Id,
            Login = usuario.Login,
            Name = usuario.Name,
            Email = usuario.Email
        };
    }
}
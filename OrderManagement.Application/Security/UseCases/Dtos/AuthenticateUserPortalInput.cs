using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Security.UseCases.Dtos;

public class AuthenticateUserPortalInput
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth;

public class RegisterDto
{
    [Required, MaxLength(150)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(150)] public string Email { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}

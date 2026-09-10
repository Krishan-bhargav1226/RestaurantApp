using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.User;

public class ForgotUserPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
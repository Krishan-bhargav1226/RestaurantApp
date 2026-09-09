using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth;

public class LoginDto
{
    [Required] public string EmailOrPhone { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

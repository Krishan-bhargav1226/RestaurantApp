using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.User;

public class RefreshUserTokenDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
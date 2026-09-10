using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.Customer;

public class RefreshCustomerTokenDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
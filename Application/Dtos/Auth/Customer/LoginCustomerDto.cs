using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.Customer;

public class LoginCustomerDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
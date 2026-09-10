using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.Customer;

public class ForgotCustomerPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
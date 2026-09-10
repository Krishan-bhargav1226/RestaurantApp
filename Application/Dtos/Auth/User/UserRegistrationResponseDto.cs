using Domain.Entities.Enums;

namespace Application.Dtos.Auth.User;

public class UserRegistrationResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsEmailVerified { get; set; }
}
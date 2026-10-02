using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Users;

public class CreateUpdateUserDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ProfileImagePath { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.Staff;

    public int? BranchId { get; set; }

    public bool IsActive { get; set; } = true;
}

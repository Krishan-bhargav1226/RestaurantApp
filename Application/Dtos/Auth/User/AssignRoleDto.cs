using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Application.Dtos.Auth.User;

public class AssignRoleDto
{
    [Required]
    public UserRole Role { get; set; }
}
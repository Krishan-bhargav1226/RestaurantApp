using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public int DisplayOrder { get; set; } = 0;
}

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Table : BaseEntity
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(20)]
    public string TableNumber { get; set; } = string.Empty;

    [Range(1, 100)]
    public int Capacity { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TableSession : BaseEntity
{
    [Required]
    public int TableId { get; set; }

    public int? CustomerId { get; set; }

    [Range(1, 100)]
    public int GuestCount { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}

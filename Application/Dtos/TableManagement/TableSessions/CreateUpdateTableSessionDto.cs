using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TableSessions;

public class CreateUpdateTableSessionDto
{
    [Range(1, int.MaxValue)]
    public int TableId { get; set; }

    [Range(1, int.MaxValue)]
    public int? CustomerId { get; set; }

    [Range(1, 100)]
    public int GuestCount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}

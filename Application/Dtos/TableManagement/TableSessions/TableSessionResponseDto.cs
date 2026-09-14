namespace Application.Dtos.TableSessions;

public class TableSessionResponseDto
{
    public int Id { get; set; }

    public int TableId { get; set; }

    public int? CustomerId { get; set; }

    public int GuestCount { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
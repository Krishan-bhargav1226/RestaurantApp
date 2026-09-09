using Domain.Entities.Enums;

namespace Application.Dtos.TableStatusHistories;

public class TableStatusHistoryResponseDto
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public TableStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? CreatedByUserId { get; set; }
    public DateTime CreatedDate { get; set; }
}

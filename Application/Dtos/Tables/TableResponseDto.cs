using Domain.Entities.Enums;

namespace Application.Dtos.Tables;

public class TableResponseDto
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

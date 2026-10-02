namespace Domain.Entities;

public class BaseEntity
{
    public int Id { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}

public abstract class TenantEntity : BaseEntity
{
    public int VenueId { get; set; }
}
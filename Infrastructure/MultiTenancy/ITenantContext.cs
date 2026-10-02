namespace Infrastructure.MultiTenancy;

public interface ITenantContext
{
    int? VenueId { get; }
    bool IsPlatformAdmin { get; }
    void SetTenant(int venueId);
    void SetPlatformAdmin();
    void Clear();
}
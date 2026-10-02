namespace Infrastructure.MultiTenancy;

public class TenantContext : ITenantContext
{
    public int? VenueId { get; private set; }
    public bool IsPlatformAdmin { get; private set; }

    public void SetTenant(int venueId)
    {
        if (venueId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(venueId));
        }

        VenueId = venueId;
        IsPlatformAdmin = false;
    }

    public void SetPlatformAdmin()
    {
        VenueId = null;
        IsPlatformAdmin = true;
    }

    public void Clear()
    {
        VenueId = null;
        IsPlatformAdmin = false;
    }
}
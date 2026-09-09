namespace Application.Dtos.CustomerAddresses;

public class CustomerAddressDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public string? Landmark { get; set; }
    public string? PinCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsDefault { get; set; }
}

namespace Application.Dtos.Branches
{
    public class BranchResponseDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string PinCode { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public bool IsOpen { get; set; }

        public bool AcceptsDineIn { get; set; }

        public bool AcceptsDelivery { get; set; }

        public decimal? DeliveryRadiusKm { get; set; }

        public decimal? MinDeliveryOrderAmount { get; set; }

        public decimal? DeliveryCharge { get; set; }

        public decimal? TaxPercentage { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}

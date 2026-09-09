using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Branches
{
    public class CreateUpdateBranchDto
    {
        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string PinCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(150)]
        [EmailAddress]
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

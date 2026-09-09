using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CustomerAddress : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Label { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string FullAddress { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Landmark { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsDefault { get; set; }
    }
}
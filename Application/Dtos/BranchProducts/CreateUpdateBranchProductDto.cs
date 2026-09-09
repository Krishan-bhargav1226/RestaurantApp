using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BranchProducts
{
    public class CreateUpdateBranchProductDto
    {
        [Required]
        public int BranchId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? Notes { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BranchProducts
{
    public class CreateUpdateBranchProductDto
    {
        [Range(1, int.MaxValue)]
        public int BranchId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? Notes { get; set; }
    }
}

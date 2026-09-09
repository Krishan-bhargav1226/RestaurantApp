namespace Application.Dtos.BranchProducts
{
    public class BranchProductResponseDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string? Notes { get; set; }
    }
}

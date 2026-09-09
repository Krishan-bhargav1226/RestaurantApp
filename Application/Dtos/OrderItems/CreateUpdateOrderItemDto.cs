using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.OrderItems
{
    public class CreateUpdateOrderItemDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [MaxLength(500)]
        public string? SpecialInstructions { get; set; }
    }
}
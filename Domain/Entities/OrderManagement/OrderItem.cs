using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class OrderItem : BaseEntity
{
    [Required]
    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? MenuItemId { get; set; }

    [MaxLength(180)]
    public string ItemNameSnapshot { get; set; } = string.Empty;

    public int? VariantId { get; set; }

    [MaxLength(100)]
    public string? VariantNameSnapshot { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    [MaxLength(500)]
    public string? SpecialInstructions { get; set; }

    public ICollection<OrderItemAddOn> AddOns { get; set; } = new List<OrderItemAddOn>();
}
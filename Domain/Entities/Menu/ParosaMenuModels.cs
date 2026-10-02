using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class MenuCategory : TenantEntity
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int SortOrder { get; set; }
}

public class MenuItem : TenantEntity
{
    public int CategoryId { get; set; }

    [Required, MaxLength(180)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public MenuItemType ItemType { get; set; } = MenuItemType.Food;

    public FoodType FoodType { get; set; } = FoodType.Veg;

    public decimal BasePrice { get; set; }

    public decimal? TaxPercentage { get; set; }

    public int PreparationTimeMinutes { get; set; }

    public int SortOrder { get; set; }

    public bool IsRecommended { get; set; }

    public bool IsOutOfStock { get; set; }

    public ICollection<ItemVariant> Variants { get; set; } = new List<ItemVariant>();

    public ICollection<MenuItemAddOn> AddOns { get; set; } = new List<MenuItemAddOn>();
}

public class ItemVariant : TenantEntity
{
    public int MenuItemId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsDefault { get; set; }

    public int SortOrder { get; set; }

    public bool IsOutOfStock { get; set; }
}

public class AddOn : TenantEntity
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsRequired { get; set; }

    public int MaxQuantity { get; set; } = 1;
}

public class MenuItemAddOn : TenantEntity
{
    public int MenuItemId { get; set; }

    public int AddOnId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsRequired { get; set; }

    public int SortOrder { get; set; }
}

public class Combo : TenantEntity
{
    [Required, MaxLength(180)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public int SortOrder { get; set; }

    public ICollection<ComboItem> Items { get; set; } = new List<ComboItem>();
}

public class ComboItem : TenantEntity
{
    public int ComboId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; } = 1;
}

public class OutletMenuItem : TenantEntity
{
    public int OutletId { get; set; }

    public int MenuItemId { get; set; }

    public decimal? PriceOverride { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsOutOfStock { get; set; }

    public TimeOnly? AvailableFrom { get; set; }

    public TimeOnly? AvailableTo { get; set; }
}
namespace MyBricks.Application.Features.ShoppingList.DTOs;

public class ShoppingListItemDto
{
    public int PartId { get; set; }
    public string RebrickablePartNum { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    
    /// <summary>
    /// Total aggregated quantity missing across all sets.
    /// </summary>
    public int TotalQuantityNeeded { get; set; }

    /// <summary>
    /// The cached Rebrickable guide price (or 0 if not found).
    /// </summary>
    public decimal EstimatedPricePerUnit { get; set; }

    public decimal EstimatedTotalPrice => TotalQuantityNeeded * EstimatedPricePerUnit;
}

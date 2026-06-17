namespace MyBricks.Domain.Entities;

/// <summary>
/// Write-through cache for Rebrickable part pricing data.
/// Prevents hammering the Rebrickable API on every shopping list generation.
/// TTL: 7 days (enforced in application layer).
/// </summary>
public class PriceCache
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public decimal AveragePriceEur { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Part Part { get; set; } = null!;
}

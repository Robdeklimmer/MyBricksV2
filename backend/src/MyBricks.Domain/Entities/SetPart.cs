namespace MyBricks.Domain.Entities;

/// <summary>
/// Join table linking a LEGO set to its constituent parts, including quantity.
/// </summary>
public class SetPart
{
    public int Id { get; set; }
    public int LegoSetId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }

    // Navigation
    public LegoSet LegoSet { get; set; } = null!;
    public Part Part { get; set; } = null!;
}

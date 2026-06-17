namespace MyBricks.Domain.Entities;

/// <summary>
/// Canonical reference record for a LEGO part, seeded from Rebrickable.
/// </summary>
public class Part
{
    public int Id { get; set; }

    /// <summary>Rebrickable part number, e.g. "3001".</summary>
    public string RebrickablePartNum { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<SetPart> SetParts { get; set; } = new List<SetPart>();
    public ICollection<MissingPart> MissingParts { get; set; } = new List<MissingPart>();
    public ICollection<VerifiedPart> VerifiedParts { get; set; } = new List<VerifiedPart>();
    public PriceCache? PriceCache { get; set; }
}

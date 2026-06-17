namespace MyBricks.Domain.Entities;

/// <summary>
/// Tracks the user's progress when building a set, checking off parts as verified/found.
/// </summary>
public class VerifiedPart
{
    public int Id { get; set; }
    public int UserSetId { get; set; }
    public int PartId { get; set; }
    public int QuantityVerified { get; set; }
    public DateTime VerifiedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public UserSet UserSet { get; set; } = null!;
    public Part Part { get; set; } = null!;
}

using MyBricks.Domain.Enums;

namespace MyBricks.Domain.Entities;

/// <summary>
/// A user-reported flag on a part within a specific owned set.
/// Tracks quantity missing/broken and whether it has been resolved (purchased).
/// </summary>
public class MissingPart
{
    public int Id { get; set; }
    public int UserSetId { get; set; }
    public int PartId { get; set; }
    public int QuantityMissing { get; set; } = 1;
    public PartCondition Condition { get; set; } = PartCondition.Missing;
    public string? Note { get; set; }
    public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Set when the user marks the part as found/purchased.</summary>
    public DateTime? ResolvedAt { get; set; }

    public bool IsResolved => ResolvedAt.HasValue;

    // Navigation
    public UserSet UserSet { get; set; } = null!;
    public Part Part { get; set; } = null!;
}

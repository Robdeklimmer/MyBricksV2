namespace MyBricks.Domain.Entities;

/// <summary>
/// Canonical reference record for a LEGO set, seeded from Rebrickable.
/// Not owned by any single user — shared across all UserSets.
/// </summary>
public class LegoSet
{
    public int Id { get; set; }

    /// <summary>Rebrickable set identifier, e.g. "75192-1".</summary>
    public string RebrickableSetNum { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Theme { get; set; } = string.Empty;
    public int TotalParts { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<SetPart> SetParts { get; set; } = new List<SetPart>();
    public ICollection<UserSet> UserSets { get; set; } = new List<UserSet>();
}

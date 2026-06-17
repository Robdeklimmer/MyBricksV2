namespace MyBricks.Domain.Entities;

/// <summary>
/// Represents a user's ownership of a specific LEGO set instance.
/// A user can own multiple copies of the same LegoSet (each is a separate UserSet).
/// Optionally linked to a FamilyGroup for shared access.
/// </summary>
public class UserSet
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LegoSetId { get; set; }
    public int? FamilyGroupId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public bool IsComplete { get; set; } = false;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public LegoSet LegoSet { get; set; } = null!;
    public FamilyGroup? FamilyGroup { get; set; }
    public ICollection<MissingPart> MissingParts { get; set; } = new List<MissingPart>();
    public ICollection<VerifiedPart> VerifiedParts { get; set; } = new List<VerifiedPart>();
}

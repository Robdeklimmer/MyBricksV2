namespace MyBricks.Domain.Entities;

/// <summary>
/// Join entity representing a user's membership in a family group.
/// </summary>
public class UserFamilyGroup
{
    public int UserId { get; set; }
    public int FamilyGroupId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public FamilyGroup FamilyGroup { get; set; } = null!;
}

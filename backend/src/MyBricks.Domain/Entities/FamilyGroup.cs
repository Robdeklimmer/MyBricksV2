namespace MyBricks.Domain.Entities;

/// <summary>
/// A family/group that multiple users can share for collaborative inventory access.
/// </summary>
public class FamilyGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Short alphanumeric code used to invite new members.</summary>
    public string InviteCode { get; set; } = string.Empty;

    public int OwnerUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser Owner { get; set; } = null!;
    public ICollection<UserFamilyGroup> UserFamilyGroups { get; set; } = new List<UserFamilyGroup>();
    public ICollection<UserSet> UserSets { get; set; } = new List<UserSet>();
}

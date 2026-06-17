using Microsoft.AspNetCore.Identity;

namespace MyBricks.Domain.Entities;

/// <summary>
/// Application user — extends ASP.NET Core Identity IdentityUser.
/// Users can belong to multiple FamilyGroups via UserFamilyGroups.
/// </summary>
public class ApplicationUser : IdentityUser<int>
{
    public string DisplayName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<UserFamilyGroup> UserFamilyGroups { get; set; } = new List<UserFamilyGroup>();
    public ICollection<UserSet> UserSets { get; set; } = new List<UserSet>();
}

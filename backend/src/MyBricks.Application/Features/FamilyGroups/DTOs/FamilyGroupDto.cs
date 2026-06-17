namespace MyBricks.Application.Features.FamilyGroups.DTOs;

public class FamilyGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InviteCode { get; set; } = string.Empty;
    public int OwnerUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

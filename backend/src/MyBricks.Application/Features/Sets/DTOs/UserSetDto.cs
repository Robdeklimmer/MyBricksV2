namespace MyBricks.Application.Features.Sets.DTOs;

public class UserSetDto
{
    public int Id { get; set; }
    public string RebrickableSetNum { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? FamilyGroupId { get; set; }
    public bool IsComplete { get; set; }
    public DateTime AddedAt { get; set; }
}

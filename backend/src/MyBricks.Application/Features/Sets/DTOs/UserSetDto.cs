namespace MyBricks.Application.Features.Sets.DTOs;

public class UserSetDto
{
    public int Id { get; set; }
    public LegoSetDto LegoSet { get; set; } = null!;
    public int? FamilyGroupId { get; set; }
    public bool IsComplete { get; set; }
    public DateTime AddedAt { get; set; }
}

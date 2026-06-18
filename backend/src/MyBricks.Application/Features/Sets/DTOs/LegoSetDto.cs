namespace MyBricks.Application.Features.Sets.DTOs;

public class LegoSetDto
{
    public int Id { get; set; }
    public string RebrickableSetNum { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Theme { get; set; } = string.Empty;
    public int TotalParts { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? LastSyncedAt { get; set; }
}

using MyBricks.Domain.Enums;

namespace MyBricks.Application.Features.Parts.DTOs;

public class MissingPartDto
{
    public int Id { get; set; }
    public int UserSetId { get; set; }
    public int PartId { get; set; }
    public string RebrickablePartNum { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int QuantityMissing { get; set; }
    public PartCondition Condition { get; set; }
    public string? Note { get; set; }
    public DateTime FlaggedAt { get; set; }
    public bool IsResolved { get; set; }
}

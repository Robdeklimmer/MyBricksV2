using System.Text.Json.Serialization;

namespace MyBricks.Infrastructure.ExternalServices.Rebrickable.Models;

public class RebrickableSetResponse
{
    [JsonPropertyName("set_num")]
    public string SetNum { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("theme_id")]
    public int ThemeId { get; set; }

    [JsonPropertyName("num_parts")]
    public int NumParts { get; set; }

    [JsonPropertyName("set_img_url")]
    public string? SetImgUrl { get; set; }
}

public class RebrickablePartResponse
{
    [JsonPropertyName("part_num")]
    public string PartNum { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("part_cat_id")]
    public int PartCatId { get; set; }

    [JsonPropertyName("part_img_url")]
    public string? PartImgUrl { get; set; }
}

public class RebrickableColorResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class RebrickableSetPartResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("inv_part_id")]
    public int InvPartId { get; set; }

    [JsonPropertyName("part")]
    public RebrickablePartResponse Part { get; set; } = new();

    [JsonPropertyName("color")]
    public RebrickableColorResponse Color { get; set; } = new();

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("is_spare")]
    public bool IsSpare { get; set; }
}

public class RebrickablePaginatedResponse<T>
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("results")]
    public List<T> Results { get; set; } = new();
}

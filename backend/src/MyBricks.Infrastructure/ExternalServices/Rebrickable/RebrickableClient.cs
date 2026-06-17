using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Domain.Entities;
using MyBricks.Infrastructure.ExternalServices.Rebrickable.Models;

namespace MyBricks.Infrastructure.ExternalServices.Rebrickable;

public class RebrickableClient : IRebrickableClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RebrickableClient> _logger;

    public RebrickableClient(HttpClient httpClient, IConfiguration configuration, ILogger<RebrickableClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        var apiKey = configuration["Rebrickable:ApiKey"];
        if (!string.IsNullOrEmpty(apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"key {apiKey}");
        }
        
        _httpClient.BaseAddress = new Uri("https://rebrickable.com/api/v3/");
    }

    public async Task<LegoSet?> GetSetDetailsAsync(string setNum, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"lego/sets/{setNum}/", ct);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<RebrickableSetResponse>(cancellationToken: ct);
            if (data == null) return null;

            return new LegoSet
            {
                RebrickableSetNum = data.SetNum,
                Name = data.Name,
                Year = data.Year,
                Theme = data.ThemeId.ToString(), // Might need a separate call to map theme ID to name, but ID works for now
                TotalParts = data.NumParts,
                ImageUrl = data.SetImgUrl,
                LastSyncedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching set details from Rebrickable for {SetNum}", setNum);
            throw;
        }
    }

    public async Task<IReadOnlyList<SetPart>> GetSetPartsAsync(string setNum, CancellationToken ct = default)
    {
        var result = new List<SetPart>();
        string? nextUrl = $"lego/sets/{setNum}/parts/?page_size=1000"; // Fetch up to 1000 at once

        try
        {
            while (!string.IsNullOrEmpty(nextUrl))
            {
                var response = await _httpClient.GetAsync(nextUrl, ct);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<RebrickablePaginatedResponse<RebrickableSetPartResponse>>(cancellationToken: ct);
                if (data == null) break;

                foreach (var item in data.Results)
                {
                    // Filter out spares to avoid inflating the parts count for the main build
                    if (item.IsSpare) continue;

                    result.Add(new SetPart
                    {
                        Quantity = item.Quantity,
                        Part = new Part
                        {
                            RebrickablePartNum = item.Part.PartNum,
                            Name = item.Part.Name,
                            Category = item.Part.PartCatId.ToString(),
                            Color = item.Color.Name,
                            ImageUrl = item.Part.PartImgUrl
                        }
                    });
                }

                nextUrl = data.Next; // Will be null when pages are exhausted
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching set parts from Rebrickable for {SetNum}", setNum);
            throw;
        }
    }
}

using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace MyBricks.Migration;

public record LegacyFamily(long Id, string Name, string CreatedBy, string? InviteCode, DateTime CreatedAt);
public record LegacyFamilyMember(long Id, long FamilyId, string UserId, string Role, DateTime JoinedAt);
public record LegacyUserSet(long Id, string Name, int NumParts, string? SetImgUrl, string SetNum, string? UserId, int Year, long? FamilyId);
public record LegacyMissingPart(long Id, int ColorId, string ColorName, int MissingQuantity, string? PartImgUrl, string PartName, string PartNum, string SetNum, int TotalQuantity, string UserId, string? Status, DateTime? StatusDate, string? StatusNotes, long? FamilyId);
public record LegacyVerifiedPart(long Id, string UserId, long? FamilyId, string SetNum, string PartNum, int ColorId, string PartName, string ColorName, string? PartImgUrl, int Quantity, DateTime? VerifiedDate);

public class LegacyData
{
    public List<LegacyFamily> Families { get; set; } = new();
    public List<LegacyFamilyMember> FamilyMembers { get; set; } = new();
    public List<LegacyUserSet> UserSets { get; set; } = new();
    public List<LegacyMissingPart> MissingParts { get; set; } = new();
    public List<LegacyVerifiedPart> VerifiedParts { get; set; } = new();
}

public class LegacyDbReader
{
    private readonly string _connectionString;
    private readonly ILogger _logger;

    public LegacyDbReader(string connectionString, ILogger logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<LegacyData> ReadAllAsync()
    {
        _logger.LogInformation("Connecting to legacy database...");
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var data = new LegacyData();

        data.Families = (await connection.QueryAsync<LegacyFamily>("SELECT id, name, created_by AS CreatedBy, invite_code AS InviteCode, created_at AS CreatedAt FROM families")).ToList();
        _logger.LogInformation("Read {Count} families.", data.Families.Count);

        data.FamilyMembers = (await connection.QueryAsync<LegacyFamilyMember>("SELECT id, family_id AS FamilyId, user_id AS UserId, role, joined_at AS JoinedAt FROM family_members")).ToList();
        _logger.LogInformation("Read {Count} family members.", data.FamilyMembers.Count);

        data.UserSets = (await connection.QueryAsync<LegacyUserSet>("SELECT id, name, num_parts AS NumParts, set_img_url AS SetImgUrl, set_num AS SetNum, user_id AS UserId, year, family_id AS FamilyId FROM user_sets")).ToList();
        _logger.LogInformation("Read {Count} user sets.", data.UserSets.Count);

        data.MissingParts = (await connection.QueryAsync<LegacyMissingPart>("SELECT id, color_id AS ColorId, color_name AS ColorName, missing_quantity AS MissingQuantity, part_img_url AS PartImgUrl, part_name AS PartName, part_num AS PartNum, set_num AS SetNum, total_quantity AS TotalQuantity, user_id AS UserId, status, status_date AS StatusDate, status_notes AS StatusNotes, family_id AS FamilyId FROM missing_parts")).ToList();
        _logger.LogInformation("Read {Count} missing parts.", data.MissingParts.Count);

        data.VerifiedParts = (await connection.QueryAsync<LegacyVerifiedPart>("SELECT id, user_id AS UserId, family_id AS FamilyId, set_num AS SetNum, part_num AS PartNum, color_id AS ColorId, part_name AS PartName, color_name AS ColorName, part_img_url AS PartImgUrl, quantity AS Quantity, verified_date AS VerifiedDate FROM verified_parts")).ToList();
        _logger.LogInformation("Read {Count} verified parts.", data.VerifiedParts.Count);

        return data;
    }
}

using MyBricks.Domain.Entities;
using MyBricks.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace MyBricks.Migration;

public class NewData
{
    public List<ApplicationUser> Users { get; set; } = new();
    public List<FamilyGroup> FamilyGroups { get; set; } = new();
    public List<UserFamilyGroup> UserFamilyGroups { get; set; } = new();
    public List<LegoSet> LegoSets { get; set; } = new();
    public List<UserSet> UserSets { get; set; } = new();
    public List<Part> Parts { get; set; } = new();
    public List<MissingPart> MissingParts { get; set; } = new();
    public List<VerifiedPart> VerifiedParts { get; set; } = new();
}

public class MigrationMapper
{
    private readonly ILogger _logger;

    public MigrationMapper(ILogger logger)
    {
        _logger = logger;
    }

    public NewData Map(LegacyData legacy)
    {
        var newData = new NewData();
        _logger.LogInformation("Mapping Legacy Data to New Data Models...");

        var distinctUserIds = legacy.FamilyMembers.Select(fm => fm.UserId)
            .Concat(legacy.UserSets.Where(us => us.UserId != null).Select(us => us.UserId!))
            .Concat(legacy.MissingParts.Where(mp => !string.IsNullOrEmpty(mp.UserId)).Select(mp => mp.UserId))
            .Concat(legacy.VerifiedParts.Where(vp => !string.IsNullOrEmpty(vp.UserId)).Select(vp => vp.UserId))
            .Concat(legacy.Families.Select(f => f.CreatedBy))
            .Distinct()
            .ToList();

        var legacyToNewUserIdMap = new Dictionary<string, int>();
        int userIdCounter = 1;

        foreach (var legacyUserId in distinctUserIds)
        {
            var newUser = new ApplicationUser
            {
                // We do NOT set Id. We let EF auto-increment it if it's identity. Wait, we need the IDs to map relationships in memory.
                // We can set Id, but we must use SET IDENTITY_INSERT or allow EF to insert. MySQL handles identity insert transparently if we provide it sometimes.
                // It's safer to not set Id, and rely on navigation properties for EF.
                // But we need a mapping to construct navigation properties.
                // We'll set the Id temporarily, but we might run into issues with MySQL auto_increment if we insert IDs.
                // For a migration script into an EMPTY DB, inserting explicit IDs is usually fine.
                Id = userIdCounter,
                UserName = legacyUserId,
                NormalizedUserName = legacyUserId.ToUpper(),
                Email = $"{legacyUserId.Replace("|", "_")}@legacy.local",
                NormalizedEmail = $"{legacyUserId.Replace("|", "_")}@LEGACY.LOCAL".ToUpper(),
                DisplayName = "Legacy User",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            newData.Users.Add(newUser);
            legacyToNewUserIdMap[legacyUserId] = userIdCounter;
            userIdCounter++;
        }

        var legacyToNewFamilyIdMap = new Dictionary<long, int>();
        int familyIdCounter = 1;

        foreach (var f in legacy.Families)
        {
            var newFam = new FamilyGroup
            {
                Id = familyIdCounter,
                Name = f.Name,
                InviteCode = f.InviteCode ?? Guid.NewGuid().ToString().Substring(0, 8),
                OwnerUserId = legacyToNewUserIdMap[f.CreatedBy],
                CreatedAt = f.CreatedAt
            };
            newData.FamilyGroups.Add(newFam);
            legacyToNewFamilyIdMap[f.Id] = familyIdCounter;
            familyIdCounter++;
        }

        foreach (var fm in legacy.FamilyMembers)
        {
            if (legacyToNewFamilyIdMap.TryGetValue(fm.FamilyId, out var newFamId) && legacyToNewUserIdMap.TryGetValue(fm.UserId, out var newUserId))
            {
                newData.UserFamilyGroups.Add(new UserFamilyGroup
                {
                    UserId = newUserId,
                    FamilyGroupId = newFamId,
                    JoinedAt = fm.JoinedAt
                });
            }
        }

        var distinctSets = legacy.UserSets
            .GroupBy(us => us.SetNum)
            .Select(g => g.First())
            .ToList();

        var setNumToNewSetIdMap = new Dictionary<string, int>();
        int setIdCounter = 1;

        foreach (var s in distinctSets)
        {
            newData.LegoSets.Add(new LegoSet
            {
                Id = setIdCounter,
                RebrickableSetNum = s.SetNum,
                Name = s.Name,
                Year = s.Year,
                TotalParts = s.NumParts,
                ImageUrl = s.SetImgUrl,
                CreatedAt = DateTime.UtcNow
            });
            setNumToNewSetIdMap[s.SetNum] = setIdCounter;
            setIdCounter++;
        }

        int userSetIdCounter = 1;
        foreach (var us in legacy.UserSets)
        {
            int userId = us.UserId != null && legacyToNewUserIdMap.TryGetValue(us.UserId, out var mappedUserId) 
                            ? mappedUserId 
                            : legacyToNewUserIdMap.Values.First();
            
            var newUserSet = new UserSet
            {
                Id = userSetIdCounter,
                UserId = userId,
                LegoSetId = setNumToNewSetIdMap[us.SetNum],
                FamilyGroupId = us.FamilyId.HasValue && legacyToNewFamilyIdMap.ContainsKey(us.FamilyId.Value) 
                                    ? legacyToNewFamilyIdMap[us.FamilyId.Value] : null,
                AddedAt = DateTime.UtcNow,
                IsComplete = false
            };
            newData.UserSets.Add(newUserSet);
            userSetIdCounter++;
        }

        var distinctParts = legacy.MissingParts.Select(mp => new { mp.PartNum, mp.PartName, mp.ColorName, mp.PartImgUrl })
            .Concat(legacy.VerifiedParts.Select(vp => new { vp.PartNum, vp.PartName, vp.ColorName, vp.PartImgUrl }))
            .GroupBy(p => new { p.PartNum, p.ColorName })
            .Select(g => g.First())
            .ToList();

        var partNumAndColorToNewPartIdMap = new Dictionary<(string, string), int>();
        int partIdCounter = 1;

        foreach (var p in distinctParts)
        {
            newData.Parts.Add(new Part
            {
                Id = partIdCounter,
                RebrickablePartNum = p.PartNum,
                Name = p.PartName,
                Color = p.ColorName,
                ImageUrl = p.PartImgUrl,
                CreatedAt = DateTime.UtcNow
            });
            partNumAndColorToNewPartIdMap[(p.PartNum, p.ColorName)] = partIdCounter;
            partIdCounter++;
        }

        int missingPartIdCounter = 1;
        foreach (var mp in legacy.MissingParts)
        {
            int? mappedUserId = mp.UserId != null && legacyToNewUserIdMap.TryGetValue(mp.UserId, out var uId) ? uId : null;
            int? mappedFamId = mp.FamilyId.HasValue && legacyToNewFamilyIdMap.TryGetValue(mp.FamilyId.Value, out var fId) ? fId : null;
            int mappedLegoSetId = setNumToNewSetIdMap[mp.SetNum];

            var matchingUserSet = newData.UserSets.FirstOrDefault(us => 
                us.LegoSetId == mappedLegoSetId && 
                ((us.FamilyGroupId == mappedFamId && mappedFamId != null) || (us.UserId == mappedUserId && mappedUserId != null))
            );

            if (matchingUserSet == null)
            {
                _logger.LogWarning("Could not find matching UserSet for missing part {PartNum} in set {SetNum}", mp.PartNum, mp.SetNum);
                continue;
            }

            newData.MissingParts.Add(new MissingPart
            {
                Id = missingPartIdCounter,
                UserSetId = matchingUserSet.Id,
                PartId = partNumAndColorToNewPartIdMap[(mp.PartNum, mp.ColorName)],
                QuantityMissing = mp.MissingQuantity,
                Condition = PartCondition.Missing,
                Note = "Legacy Status: " + mp.Status + (mp.StatusNotes != null ? " - " + mp.StatusNotes : ""),
                FlaggedAt = mp.StatusDate ?? DateTime.UtcNow,
                ResolvedAt = (mp.Status == "delivered" || mp.Status == "shipped") ? mp.StatusDate : null
            });
            missingPartIdCounter++;
        }

        int verifiedPartIdCounter = 1;
        foreach (var vp in legacy.VerifiedParts)
        {
            int? mappedUserId = vp.UserId != null && legacyToNewUserIdMap.TryGetValue(vp.UserId, out var uId) ? uId : null;
            int? mappedFamId = vp.FamilyId.HasValue && legacyToNewFamilyIdMap.TryGetValue(vp.FamilyId.Value, out var fId) ? fId : null;
            int mappedLegoSetId = setNumToNewSetIdMap[vp.SetNum];

            var matchingUserSet = newData.UserSets.FirstOrDefault(us => 
                us.LegoSetId == mappedLegoSetId && 
                ((us.FamilyGroupId == mappedFamId && mappedFamId != null) || (us.UserId == mappedUserId && mappedUserId != null))
            );

            if (matchingUserSet == null)
            {
                _logger.LogWarning("Could not find matching UserSet for verified part {PartNum} in set {SetNum}", vp.PartNum, vp.SetNum);
                continue;
            }

            newData.VerifiedParts.Add(new VerifiedPart
            {
                Id = verifiedPartIdCounter,
                UserSetId = matchingUserSet.Id,
                PartId = partNumAndColorToNewPartIdMap[(vp.PartNum, vp.ColorName)],
                QuantityVerified = vp.Quantity,
                VerifiedAt = vp.VerifiedDate ?? DateTime.UtcNow
            });
            verifiedPartIdCounter++;
        }

        _logger.LogInformation("Mapped: {C} Users, {C2} Families, {C3} Sets, {C4} UserSets, {C5} Parts, {C6} MissingParts, {C7} VerifiedParts", 
            newData.Users.Count, newData.FamilyGroups.Count, newData.LegoSets.Count, newData.UserSets.Count, newData.Parts.Count, newData.MissingParts.Count, newData.VerifiedParts.Count);

        return newData;
    }
}

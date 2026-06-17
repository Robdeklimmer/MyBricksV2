using MyBricks.Domain.Entities;

namespace MyBricks.Domain.Interfaces;

public interface IMissingPartRepository
{
    Task<MissingPart?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<MissingPart>> GetByUserSetIdAsync(int userSetId, CancellationToken ct = default);

    /// <summary>
    /// Aggregates all unresolved missing parts for a user's personal lists (where FamilyGroupId is null).
    /// </summary>
    Task<IReadOnlyList<MissingPart>> GetUnresolvedPersonalAsync(int userId, CancellationToken ct = default);

    /// <summary>
    /// Aggregates all unresolved missing parts across an entire family group.
    /// Used by the shopping list generator.
    /// </summary>
    Task<IReadOnlyList<MissingPart>> GetUnresolvedByGroupIdAsync(int groupId, CancellationToken ct = default);

    Task<MissingPart> AddAsync(MissingPart missingPart, CancellationToken ct = default);
    Task UpdateAsync(MissingPart missingPart, CancellationToken ct = default);
}

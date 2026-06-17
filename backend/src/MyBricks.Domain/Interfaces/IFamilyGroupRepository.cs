using MyBricks.Domain.Entities;

namespace MyBricks.Domain.Interfaces;

public interface IFamilyGroupRepository
{
    Task<FamilyGroup?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<FamilyGroup?> GetByInviteCodeAsync(string inviteCode, CancellationToken ct = default);
    Task<IReadOnlyList<FamilyGroup>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<FamilyGroup> AddAsync(FamilyGroup group, CancellationToken ct = default);
    Task UpdateAsync(FamilyGroup group, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

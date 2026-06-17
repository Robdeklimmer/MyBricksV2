using MyBricks.Domain.Entities;

namespace MyBricks.Domain.Interfaces;

public interface IUserSetRepository
{
    Task<UserSet?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<UserSet>> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<IReadOnlyList<UserSet>> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
    Task<UserSet> AddAsync(UserSet userSet, CancellationToken ct = default);
    Task UpdateAsync(UserSet userSet, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

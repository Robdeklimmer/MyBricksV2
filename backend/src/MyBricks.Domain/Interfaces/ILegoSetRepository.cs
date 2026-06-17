using MyBricks.Domain.Entities;

namespace MyBricks.Domain.Interfaces;

public interface ILegoSetRepository
{
    Task<LegoSet?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<LegoSet?> GetByRebrickableNumAsync(string setNum, CancellationToken ct = default);
    Task<IReadOnlyList<LegoSet>> GetAllAsync(CancellationToken ct = default);
    Task<LegoSet> AddAsync(LegoSet legoSet, CancellationToken ct = default);
    Task UpdateAsync(LegoSet legoSet, CancellationToken ct = default);
    Task<bool> ExistsAsync(string setNum, CancellationToken ct = default);
}

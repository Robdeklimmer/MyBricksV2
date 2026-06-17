using MyBricks.Domain.Entities;

namespace MyBricks.Application.Common.Interfaces;

public interface IRebrickableClient
{
    Task<LegoSet?> GetSetDetailsAsync(string setNum, CancellationToken ct = default);
    Task<IReadOnlyList<SetPart>> GetSetPartsAsync(string setNum, CancellationToken ct = default);
}

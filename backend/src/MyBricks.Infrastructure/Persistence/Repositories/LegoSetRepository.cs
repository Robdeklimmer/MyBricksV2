using Microsoft.EntityFrameworkCore;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Infrastructure.Persistence.Repositories;

public class LegoSetRepository : ILegoSetRepository
{
    private readonly ApplicationDbContext _context;

    public LegoSetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LegoSet?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.LegoSets
            .Include(s => s.SetParts)
                .ThenInclude(sp => sp.Part)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<LegoSet?> GetByRebrickableNumAsync(string setNum, CancellationToken ct = default)
    {
        return await _context.LegoSets
            .Include(s => s.SetParts)
                .ThenInclude(sp => sp.Part)
            .FirstOrDefaultAsync(s => s.RebrickableSetNum == setNum, ct);
    }

    public async Task<IReadOnlyList<LegoSet>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.LegoSets.ToListAsync(ct);
    }

    public async Task<LegoSet> AddAsync(LegoSet legoSet, CancellationToken ct = default)
    {
        await _context.LegoSets.AddAsync(legoSet, ct);
        return legoSet;
    }

    public Task UpdateAsync(LegoSet legoSet, CancellationToken ct = default)
    {
        _context.LegoSets.Update(legoSet);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string setNum, CancellationToken ct = default)
    {
        return await _context.LegoSets.AnyAsync(s => s.RebrickableSetNum == setNum, ct);
    }
}

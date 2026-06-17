using Microsoft.EntityFrameworkCore;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Infrastructure.Persistence.Repositories;

public class MissingPartRepository : IMissingPartRepository
{
    private readonly ApplicationDbContext _context;

    public MissingPartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MissingPart?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.MissingParts
            .Include(m => m.Part)
                .ThenInclude(p => p.PriceCache)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<IReadOnlyList<MissingPart>> GetByUserSetIdAsync(int userSetId, CancellationToken ct = default)
    {
        return await _context.MissingParts
            .Include(m => m.Part)
                .ThenInclude(p => p.PriceCache)
            .Where(m => m.UserSetId == userSetId)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<MissingPart>> GetUnresolvedPersonalAsync(int userId, CancellationToken ct = default)
    {
        return await _context.MissingParts
            .Include(m => m.Part)
                .ThenInclude(p => p.PriceCache)
            .Where(m => m.ResolvedAt == null 
                     && m.UserSet.UserId == userId 
                     && m.UserSet.FamilyGroupId == null)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<MissingPart>> GetUnresolvedByGroupIdAsync(int groupId, CancellationToken ct = default)
    {
        return await _context.MissingParts
            .Include(m => m.Part)
                .ThenInclude(p => p.PriceCache)
            .Where(m => m.ResolvedAt == null && m.UserSet.FamilyGroupId == groupId)
            .ToListAsync(ct);
    }

    public async Task<MissingPart> AddAsync(MissingPart missingPart, CancellationToken ct = default)
    {
        await _context.MissingParts.AddAsync(missingPart, ct);
        return missingPart;
    }

    public Task UpdateAsync(MissingPart missingPart, CancellationToken ct = default)
    {
        _context.MissingParts.Update(missingPart);
        return Task.CompletedTask;
    }
}

using Microsoft.EntityFrameworkCore;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Infrastructure.Persistence.Repositories;

public class FamilyGroupRepository : IFamilyGroupRepository
{
    private readonly ApplicationDbContext _context;

    public FamilyGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FamilyGroup?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.FamilyGroups
            .Include(g => g.UserFamilyGroups)
            .FirstOrDefaultAsync(g => g.Id == id, ct);
    }

    public async Task<FamilyGroup?> GetByInviteCodeAsync(string inviteCode, CancellationToken ct = default)
    {
        return await _context.FamilyGroups
            .Include(g => g.UserFamilyGroups)
            .FirstOrDefaultAsync(g => g.InviteCode == inviteCode, ct);
    }

    public async Task<IReadOnlyList<FamilyGroup>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await _context.FamilyGroups
            .Where(g => g.UserFamilyGroups.Any(ug => ug.UserId == userId))
            .ToListAsync(ct);
    }

    public async Task<FamilyGroup> AddAsync(FamilyGroup group, CancellationToken ct = default)
    {
        await _context.FamilyGroups.AddAsync(group, ct);
        return group;
    }

    public Task UpdateAsync(FamilyGroup group, CancellationToken ct = default)
    {
        _context.FamilyGroups.Update(group);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var group = await _context.FamilyGroups.FindAsync(new object[] { id }, ct);
        if (group != null)
        {
            _context.FamilyGroups.Remove(group);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Infrastructure.Persistence.Repositories;

public class UserSetRepository : IUserSetRepository
{
    private readonly ApplicationDbContext _context;

    public UserSetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserSet?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.UserSets
            .Include(u => u.LegoSet)
            .Include(u => u.MissingParts)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<IReadOnlyList<UserSet>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await _context.UserSets
            .Include(u => u.LegoSet)
            .Where(u => u.UserId == userId) // All sets belonging to the user
            .OrderByDescending(u => u.AddedAt)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<UserSet>> GetByGroupIdAsync(int groupId, CancellationToken ct = default)
    {
        return await _context.UserSets
            .Include(u => u.LegoSet)
            .Where(u => u.FamilyGroupId == groupId)
            .ToListAsync(ct);
    }

    public async Task<UserSet> AddAsync(UserSet userSet, CancellationToken ct = default)
    {
        await _context.UserSets.AddAsync(userSet, ct);
        return userSet;
    }

    public Task UpdateAsync(UserSet userSet, CancellationToken ct = default)
    {
        _context.UserSets.Update(userSet);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var userSet = await _context.UserSets.FindAsync(new object[] { id }, ct);
        if (userSet != null)
        {
            _context.UserSets.Remove(userSet);
        }
    }
}

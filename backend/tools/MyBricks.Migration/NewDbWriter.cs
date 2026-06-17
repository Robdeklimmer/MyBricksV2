using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBricks.Infrastructure.Persistence;

namespace MyBricks.Migration;

public class NewDbWriter
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;

    public NewDbWriter(ApplicationDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task WriteAsync(NewData data)
    {
        _logger.LogInformation("Starting database write...");

        // Disable foreign key checks for bulk insert with explicit IDs
        await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS=0;");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Clear existing data? We should probably just append or assume it's a fresh DB
            // Let's assume a fresh DB for the migration

            _logger.LogInformation("Writing Users...");
            await _context.Users.AddRangeAsync(data.Users);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing Families...");
            await _context.FamilyGroups.AddRangeAsync(data.FamilyGroups);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing UserFamilyGroups...");
            await _context.UserFamilyGroups.AddRangeAsync(data.UserFamilyGroups);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing LegoSets...");
            await _context.LegoSets.AddRangeAsync(data.LegoSets);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing UserSets...");
            await _context.UserSets.AddRangeAsync(data.UserSets);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing Parts...");
            await _context.Parts.AddRangeAsync(data.Parts);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing MissingParts...");
            await _context.MissingParts.AddRangeAsync(data.MissingParts);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Writing VerifiedParts...");
            await _context.VerifiedParts.AddRangeAsync(data.VerifiedParts);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Successfully wrote all data!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to write data");
            throw;
        }
        finally
        {
            await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS=1;");
        }
    }
}

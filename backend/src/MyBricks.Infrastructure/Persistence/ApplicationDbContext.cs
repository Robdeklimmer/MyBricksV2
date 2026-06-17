using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FamilyGroup> FamilyGroups => Set<FamilyGroup>();
    public DbSet<UserFamilyGroup> UserFamilyGroups => Set<UserFamilyGroup>();
    public DbSet<LegoSet> LegoSets => Set<LegoSet>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<SetPart> SetParts => Set<SetPart>();
    public DbSet<UserSet> UserSets => Set<UserSet>();
    public DbSet<MissingPart> MissingParts => Set<MissingPart>();
    public DbSet<VerifiedPart> VerifiedParts => Set<VerifiedPart>();
    public DbSet<PriceCache> PriceCaches => Set<PriceCache>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

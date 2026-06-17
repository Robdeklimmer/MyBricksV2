using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class UserFamilyGroupConfiguration : IEntityTypeConfiguration<UserFamilyGroup>
{
    public void Configure(EntityTypeBuilder<UserFamilyGroup> builder)
    {
        builder.HasKey(x => new { x.UserId, x.FamilyGroupId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserFamilyGroups)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.FamilyGroup)
            .WithMany(x => x.UserFamilyGroups)
            .HasForeignKey(x => x.FamilyGroupId);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class UserSetConfiguration : IEntityTypeConfiguration<UserSet>
{
    public void Configure(EntityTypeBuilder<UserSet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserSets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LegoSet)
            .WithMany(x => x.UserSets)
            .HasForeignKey(x => x.LegoSetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FamilyGroup)
            .WithMany(x => x.UserSets)
            .HasForeignKey(x => x.FamilyGroupId)
            .OnDelete(DeleteBehavior.SetNull); // If group is deleted, sets become personal
    }
}

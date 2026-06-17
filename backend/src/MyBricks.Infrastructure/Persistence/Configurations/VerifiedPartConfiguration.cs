using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class VerifiedPartConfiguration : IEntityTypeConfiguration<VerifiedPart>
{
    public void Configure(EntityTypeBuilder<VerifiedPart> builder)
    {
        builder.HasKey(vp => vp.Id);

        builder.HasOne(vp => vp.UserSet)
            .WithMany(us => us.VerifiedParts)
            .HasForeignKey(vp => vp.UserSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vp => vp.Part)
            .WithMany(p => p.VerifiedParts)
            .HasForeignKey(vp => vp.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

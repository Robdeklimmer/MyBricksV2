using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class MissingPartConfiguration : IEntityTypeConfiguration<MissingPart>
{
    public void Configure(EntityTypeBuilder<MissingPart> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Note)
            .HasMaxLength(500);

        builder.HasOne(x => x.UserSet)
            .WithMany(x => x.MissingParts)
            .HasForeignKey(x => x.UserSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Part)
            .WithMany(x => x.MissingParts)
            .HasForeignKey(x => x.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

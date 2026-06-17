using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class SetPartConfiguration : IEntityTypeConfiguration<SetPart>
{
    public void Configure(EntityTypeBuilder<SetPart> builder)
    {
        builder.HasKey(x => x.Id);

        // A LegoSet can have many Parts, and a Part can belong to many LegoSets.
        // This is the explicit join table.
        builder.HasOne(x => x.LegoSet)
            .WithMany(x => x.SetParts)
            .HasForeignKey(x => x.LegoSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Part)
            .WithMany(x => x.SetParts)
            .HasForeignKey(x => x.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

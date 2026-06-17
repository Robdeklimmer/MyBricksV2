using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RebrickablePartNum)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RebrickablePartNum); // Not necessarily unique across colors

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Color)
            .HasMaxLength(50);

        builder.Property(x => x.Category)
            .HasMaxLength(100);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);
            
        // One-to-one with PriceCache
        builder.HasOne(x => x.PriceCache)
            .WithOne(x => x.Part)
            .HasForeignKey<PriceCache>(x => x.PartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

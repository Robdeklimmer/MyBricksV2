using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class PriceCacheConfiguration : IEntityTypeConfiguration<PriceCache>
{
    public void Configure(EntityTypeBuilder<PriceCache> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AveragePriceEur)
            .HasColumnType("decimal(18,4)");

        // One-to-one is already configured on the Part side, 
        // but can be safely re-stated here if desired.
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBricks.Domain.Entities;

namespace MyBricks.Infrastructure.Persistence.Configurations;

public class LegoSetConfiguration : IEntityTypeConfiguration<LegoSet>
{
    public void Configure(EntityTypeBuilder<LegoSet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RebrickableSetNum)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RebrickableSetNum).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Theme)
            .HasMaxLength(100);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Truss.Modeling.Application.Tests.TestCore.Domain;

namespace Truss.Modeling.Application.Tests.EfCore.Persistence;

public sealed class AutoShopConfiguration 
    : IEntityTypeConfiguration<AutoShop>
{
    public void Configure(EntityTypeBuilder<AutoShop> builder)
    {
        builder.ToTable("AutoShops");
        builder.HasKey(autoShop => autoShop.Id);

        builder.HasMany(autoShop => autoShop.Garages);

        builder.Property(autoShop => autoShop.Id)
            .HasConversion(
                id => id.Value,
                value => new AutoShopId(value)
            );

        builder.Property(autoShop => autoShop.Name);
    }
}
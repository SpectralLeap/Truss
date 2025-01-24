using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Truss.Modeling.Application.Tests.TestCore.Domain;

namespace Truss.Infrastructure.EfCore.Tests.Persistence;

public sealed class GarageConfiguration
    : IEntityTypeConfiguration<Garage>
{
    public void Configure(EntityTypeBuilder<Garage> builder)
    {
        builder.ToTable("Garages");
                
        builder.HasKey(garage => garage.Id);
        
        builder.HasMany(garage => garage.Cars);

        builder.Property(garage => garage.Id)
            .HasConversion(
                id => id.Value,
                value => new GarageId(value)
            );

    }
}
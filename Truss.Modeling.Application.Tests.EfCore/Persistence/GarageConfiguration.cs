using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Truss.Modeling.Application.Tests.EfCore.TestCore.Domain;

namespace Truss.Modeling.Application.Tests.EfCore.TestCore.Persistence;

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
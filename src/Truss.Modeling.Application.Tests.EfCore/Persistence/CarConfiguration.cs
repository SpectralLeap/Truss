using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Truss.Modeling.Application.Tests.TestCore.Domain;

namespace Truss.Modeling.Application.Tests.EfCore.Persistence;

public sealed class CarConfiguration
    : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");
                
        builder.HasKey(car => car.Id);
        
        builder.Property(car => car.Id)
            .HasConversion(
                id => id.Value,
                value => new CarId(value)
            );

    }
}
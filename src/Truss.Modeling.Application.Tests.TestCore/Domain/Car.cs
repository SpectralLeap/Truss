using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.TestCore.Domain;

public sealed class Car : Entity<CarId>
{
    public Car(CarId id)
    {
        Id = id;
    }
}
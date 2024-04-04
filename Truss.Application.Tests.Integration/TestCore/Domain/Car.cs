using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration.TestCore.Domain;

public sealed class Car : Entity<CarId>
{
    public Car(CarId id) : base(id)
    {
    }
}
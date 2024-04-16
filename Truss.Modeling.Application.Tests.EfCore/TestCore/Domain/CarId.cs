using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.EfCore.TestCore.Domain;

public sealed record CarId : EntityId<int>
{
    public CarId(int Value) : base(Value)
    {
    }
}
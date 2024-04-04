using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration.TestCore.Domain;

public sealed record CarId : EntityId<int>
{
    public CarId(int Value) : base(Value)
    {
    }
}
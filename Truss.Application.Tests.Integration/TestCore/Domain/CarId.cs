using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Tests.Integration;

public sealed record CarId : EntityId<int>
{
    public CarId(int Value) : base(Value)
    {
    }
}
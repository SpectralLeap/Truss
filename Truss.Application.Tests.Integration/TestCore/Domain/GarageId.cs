using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Tests.Integration;

public sealed record GarageId : EntityId<Guid>
{
    public GarageId(Guid Value) : base(Value)
    {
    }
}
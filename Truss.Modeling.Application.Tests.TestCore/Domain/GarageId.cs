using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.TestCore.Domain;

public sealed record GarageId : EntityId<Guid>
{
    public GarageId(Guid Value) : base(Value)
    {
    }
}
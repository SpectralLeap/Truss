using Truss.Domain.Entities;

namespace Truss.Application.Tests.Integration.TestCore.Domain;

public sealed record GarageId : EntityId<Guid>
{
    public GarageId(Guid Value) : base(Value)
    {
    }
}
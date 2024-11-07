using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed record NotWordAggregateId(Guid value) : AggregateRootId<Guid>(value)
{
    public Guid value { get; } = value;
}
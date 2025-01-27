using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed class NotWordAggregate : Aggregate<NotWordAggregateId>
{
    public NotWordAggregate(NotWordAggregateId id) : base(id)
    {
    }
}
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

internal sealed class NotWordAggregate : AggregateRoot<NotWordAggregateId>
{
    public NotWordAggregate(NotWordAggregateId id) : base(id)
    {
    }
}
using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Entities.TestDomain;

internal sealed class NotWordAggregate : AggregateRoot<NotWordAggregateId, Guid>
{
    public NotWordAggregate(NotWordAggregateId id) : base(id)
    {
    }
}
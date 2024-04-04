using Truss.Domain.EventSourcing;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record CounterCreatedEvent : CreationEvent<CounterId>
{
    public CounterCreatedEvent(Guid aggregateId) 
        : base(aggregateId)
    {
    }
}
using Truss.Application.Abstractions.EventSourcing.Writing;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record CounterCreatedEvent : CreationEvent<CounterId>
{
    public CounterCreatedEvent(Guid aggregateId) 
        : base(aggregateId)
    {
    }
}
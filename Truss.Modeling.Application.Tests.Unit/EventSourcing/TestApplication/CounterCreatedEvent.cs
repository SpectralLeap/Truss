using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.Tests.Unit.EventSourcing.TestApplication;

internal sealed record CounterCreatedEvent : CreationEvent<CounterId>
{
    public CounterCreatedEvent(Guid aggregateId) 
        : base(aggregateId)
    {
    }
}
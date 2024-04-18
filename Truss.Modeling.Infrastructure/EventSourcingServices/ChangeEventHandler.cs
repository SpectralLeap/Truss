using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Infrastructure.EventSourcingServices;

internal sealed class ChangeEventHandler<TChangeEvent>
    : IChangeEventHandler<TChangeEvent> 
    where TChangeEvent : IChangeEvent
{
    private readonly IAggregateEventStreamWriter _aggregateEventStreamWriter;

    public ChangeEventHandler(IAggregateEventStreamWriter aggregateEventStreamWriter)
    {
        _aggregateEventStreamWriter = aggregateEventStreamWriter;
    }
    
    public async Task Handle(TChangeEvent domainEvent, CancellationToken cancellationToken)
    {
        await _aggregateEventStreamWriter.Write(domainEvent);
    }
}
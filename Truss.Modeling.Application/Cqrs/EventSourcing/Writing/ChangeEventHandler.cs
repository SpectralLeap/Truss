using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// For receiving and acting on change events
/// </summary>
/// <typeparam name="TChangeEvent"></typeparam>
public interface IChangeEventHandler<in TChangeEvent> 
    where TChangeEvent : IChangeEvent
{
    public Task Handle(TChangeEvent domainEvent, CancellationToken cancellationToken);
}

public sealed class ChangeEventHandler
    : IChangeEventHandler<ChangeEvent>
{
    private readonly IAggregateEventStreamWriter _aggregateEventStreamWriter;

    public ChangeEventHandler(IAggregateEventStreamWriter aggregateEventStreamWriter)
    {
        _aggregateEventStreamWriter = aggregateEventStreamWriter;
    }
    
    public async Task Handle(ChangeEvent domainEvent, CancellationToken cancellationToken)
    {
        await _aggregateEventStreamWriter.Write(domainEvent);
    }
}
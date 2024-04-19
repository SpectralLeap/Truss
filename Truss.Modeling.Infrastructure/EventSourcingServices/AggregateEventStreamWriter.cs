using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.EventSourcingServices;

internal sealed class AggregateEventStreamWriter 
    : IAggregateEventStreamWriter
{
    private readonly IEventStore _eventStore;
    private readonly ChangeEventSerializer _serializer;
    private readonly ChangeEventTypeMap _typeMap;

    public AggregateEventStreamWriter(
        IEventStore eventStore,
        ChangeEventSerializer serializer,
        ChangeEventTypeMap typeMap
    )
    {
        _eventStore = eventStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }

    public async Task<Result<Nil>> WriteToStream<TId>(
        IEventSourcedAggregateRoot<TId> aggregate,
        CancellationToken cancellationToken = new()
    )
        where TId : AggregateRootId<Guid>
    {
        var events = aggregate.PendingChangeEvents;
        
        foreach (var changeEvent in events)
        {
            var writeableChangeEvent = new ChangeEventPayload(
                aggregate.Id.Value,
                _typeMap.Map(changeEvent.GetType()),
                _serializer.Serialize(changeEvent)
            );
        
            await _eventStore.Write(
                writeableChangeEvent,
                cancellationToken
            );
        }
        
        return Result.Success();
    }

}
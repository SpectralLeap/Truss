using Truss.Application.Cqrs.EventSourcing.Events;
using Truss.Domain.Entities;
using Truss.Domain.EventSourcing;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Writing;

internal sealed class AggregateEventStreamWriter 
    : IAggregateEventStreamWriter
{
    private readonly IEventWriteStore _eventWriteStore;
    private readonly ChangeEventSerializer _serializer;
    private readonly ChangeEventTypeMap _typeMap;

    public AggregateEventStreamWriter(
        IEventWriteStore eventWriteStore,
        ChangeEventSerializer serializer,
        ChangeEventTypeMap typeMap
    )
    {
        _eventWriteStore = eventWriteStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }

    public async Task<Result<Nil>> WriteToStream<TId>(
        IEventSourcedAggregateRoot<TId> aggregate
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
        
            await _eventWriteStore.Write(writeableChangeEvent);
        }
        
        return Result.Success();
    }

}
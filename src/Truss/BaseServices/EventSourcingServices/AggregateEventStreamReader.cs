using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.BaseServices.EventSourcingServices;

internal sealed class AggregateEventStreamReader : IAggregateEventStreamReader
{
    private readonly IEventStore _eventStore;
    private readonly ChangeEventDeserializer _deserializer;
    private readonly ChangeEventTypeMap _typeMap;

    public AggregateEventStreamReader(
        IEventStore eventStore,
        ChangeEventDeserializer deserializer,
        ChangeEventTypeMap typeMap
    )
    {
        _eventStore = eventStore;
        _deserializer = deserializer;
        _typeMap = typeMap;
    }
    
    public Result<IAsyncEnumerable<ChangeEvent>> ReadEventStream(AggregateRootId<Guid> id)
    {
        try
        {
            var storedEvents = _eventStore.Read(id);
            return Result.Success(Read(storedEvents));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    private async IAsyncEnumerable<ChangeEvent> Read(IAsyncEnumerable<ChangeEventPayload> storedEvents)
    {
         await foreach (var storedEvent in storedEvents)
         {
             var @event = _deserializer.Deserialize(
                 _typeMap.Map(storedEvent.EventType), storedEvent.SerializedPayload);
             yield return (@event);
         }       
    }
}
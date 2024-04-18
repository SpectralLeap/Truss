using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.EventSourcingServices;

internal sealed class AggregateEventStreamReader : IAggregateEventStreamReader
{
    private readonly IEventReadStore _eventReadStore;
    private readonly ChangeEventDeserializer _deserializer;
    private readonly ChangeEventTypeMap _typeMap;

    public AggregateEventStreamReader(
        IEventReadStore eventReadStore,
        ChangeEventDeserializer deserializer,
        ChangeEventTypeMap typeMap
    )
    {
        _eventReadStore = eventReadStore;
        _deserializer = deserializer;
        _typeMap = typeMap;
    }
    
    public Result<IAsyncEnumerable<ChangeEvent>> ReadEventStream(AggregateRootId<Guid> id)
    {
        try
        {
            var storedEvents = _eventReadStore.Read(id);
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
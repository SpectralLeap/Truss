using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Reading;

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
    
    public async Task<Result<IAsyncEnumerable<ChangeEvent>>> ReadEventStream(AggregateRootId<Guid> id)
    {
        return await _eventReadStore.Read(id)
                .Then(events =>
                        events.Select(e =>
                            _deserializer.Deserialize(_typeMap.Map(e.EventType), e.SerializedPayload))
                )
                .ConfigureAwait(false)
            ;
    }
}
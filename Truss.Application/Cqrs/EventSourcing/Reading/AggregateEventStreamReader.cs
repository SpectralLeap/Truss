using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.EventSourcing.Reading;
using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Application.Cqrs.EventSourcing.Events;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Reading;

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
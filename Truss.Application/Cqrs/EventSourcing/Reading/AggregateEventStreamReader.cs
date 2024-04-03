using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.EventSourcing.Reading;
using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Reading;

internal sealed class AggregateEventStreamReader : IAggregateEventStreamReader
{
    private readonly IEventStore _eventStore;
    private readonly IChangeEventSerializer _serializer;
    private readonly IChangeEventTypeMap _typeMap;

    public AggregateEventStreamReader(IEventStore eventStore, IChangeEventSerializer serializer, IChangeEventTypeMap typeMap)
    {
        _eventStore = eventStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }
    
    public async Task<Result<IAsyncEnumerable<ChangeEvent>>> ReadEventStream(AggregateRootId<Guid> id)
    {
        return await _eventStore.Read(id)
                .Then(events =>
                        events.Select(e =>
                            _serializer.Deserialize(_typeMap.Map(e.EventType), e.SerializedPayload))
                )
                .ConfigureAwait(false)
            ;
    }
}
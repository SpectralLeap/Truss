using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Application.Cqrs.EventSourcing.Reading;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Writing;

internal sealed class AggregateEventStreamWriter : IAggregateEventStreamWriter
{
    private readonly IEventStore _eventStore;
    private readonly IChangeEventSerializer _serializer;
    private readonly IChangeEventTypeMap _typeMap;

    public AggregateEventStreamWriter(IEventStore eventStore, IChangeEventSerializer serializer, IChangeEventTypeMap typeMap)
    {
        _eventStore = eventStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }

    public async Task<Result<None>> WriteToStream<TId>(IEventSourcedAggregateRoot<TId> aggregate)
        where TId : AggregateRootId<Guid>
    {
        return await WriteToStream(aggregate.Id, aggregate.PendingChangeEvents);
    }

    private async Task<Result<None>> WriteToStream(AggregateRootId<Guid> aggregateId, IEnumerable<ChangeEvent> events)
    {
        foreach (var changeEvent in events)
        {
            var writeableChangeEvent = new ChangeEventPayload(
                aggregateId.Value,
                _typeMap.Map(changeEvent.GetType()),
                _serializer.Serialize(changeEvent)
            );

            await _eventStore.Write(writeableChangeEvent);
        }

        return Result.Success();
    }
}
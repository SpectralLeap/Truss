using Newtonsoft.Json;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.Entities;
using Truss.Monads.Results;

namespace Truss.Tests.Infrastructure.InMemory.Events;

public sealed class InMemoryEventStore : IEventWriteStore, IEventReadStore
{
    private readonly Dictionary<string, Dictionary<Guid, List<string>>> _eventStreams = new();

    public Task<Result<Nil>> Write(ChangeEventPayload @event)
    {
        if (!_eventStreams.ContainsKey(@event.EventType))
        {
            _eventStreams.Add(@event.EventType, new Dictionary<Guid, List<string>>());
        }

        if (!_eventStreams[@event.EventType].ContainsKey(@event.AggregateId))
        {
            _eventStreams[@event.EventType].Add(@event.AggregateId, new List<string>());
        }

        var eventStream = _eventStreams[@event.EventType][@event.AggregateId];
        
        eventStream.Add(JsonConvert.SerializeObject(@event));

        return Task.FromResult(Result.Success());
    }

    public async Task<Result<IAsyncEnumerable<ChangeEventPayload>>> Read(AggregateRootId<Guid> id)
    {
        var events = GetEvents(id.Value).ToAsyncEnumerable();

        if (await events.IsEmptyAsync()) return Result.Fail("Empty");
        return Result.Success(events);
    }
    
    private IEnumerable<ChangeEventPayload> GetEvents(Guid aggregateId) =>
        _eventStreams.Values.SelectMany(stream => 
            stream.ContainsKey(aggregateId) ?
                stream[aggregateId].Select(JsonConvert.DeserializeObject<ChangeEventPayload>).ToList()! :
                new List<ChangeEventPayload>());


}
using Newtonsoft.Json;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.Entities;
using Truss.Monads.Results;

namespace Truss.Tests.Infrastructure.InMemory.Events;

public sealed class InMemoryEventStore : IEventStore
{
    private readonly Dictionary<string, Dictionary<Guid, List<string>>> _eventStreams = new();

    public Task<Result<Nil>> Write(ChangeEventPayload @event, CancellationToken cancellationToken)
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

    public async IAsyncEnumerable<ChangeEventPayload> Read(AggregateRootId<Guid> aggregateId)
    {
        var events = _eventStreams.Values.SelectMany(stream => 
            stream.ContainsKey(aggregateId) ?
                stream[aggregateId].Select(JsonConvert.DeserializeObject<ChangeEventPayload>).ToList()! :
                new List<ChangeEventPayload>());

        foreach (var @event in events)
        {
            yield return @event;
        }
    }
}
using Newtonsoft.Json;
using Truss.Application.Abstractions.Events;

namespace Truss.Application.Cqrs.EventSourcing.Common;

public sealed record ChangeEventPayload
{
    [JsonProperty] 
    public const int Version = 1;
    public Guid AggregateId { get; }
    public EventType EventType { get; }
    public string SerializedPayload { get; }
    
    public ChangeEventPayload(Guid aggregateId, EventType eventType, string serializedPayload)
    {
        AggregateId = aggregateId;
        EventType = eventType;
        SerializedPayload = serializedPayload;
    }

}
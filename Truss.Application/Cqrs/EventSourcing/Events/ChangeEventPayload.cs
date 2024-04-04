using Newtonsoft.Json;

namespace Truss.Application.Cqrs.EventSourcing.Events;

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
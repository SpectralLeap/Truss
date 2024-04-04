using Newtonsoft.Json;

namespace Truss.Domain.EventSourcing;

/// <summary>
/// Represents an aggregate's state has changed
/// </summary>
public abstract record ChangeEvent 
    : IChangeEvent
{
    /// <summary>
    /// For general event versioning
    /// </summary>
    [JsonProperty]
    public int ChangeEventVersion { get; private set; } = 1;
            
    /// <summary>
    /// The unique id of the event
    /// </summary>
    [JsonProperty]
    public Guid Id { get; private set; } = Guid.NewGuid();   
    
    /// <summary>
    /// Represents the unique identifier of an aggregate.
    /// </summary>
    [JsonProperty]
    public Guid AggregateId { get; private set; }

    /// <summary>
    /// When the event occured
    /// </summary>
    [JsonProperty]
    public DateTime Time { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// The order of the event in an aggregate's event sequence
    /// </summary>
    [JsonProperty]
    public EventSequenceNumber? EventSequenceNumber { get; private set; }

    /// <summary>
    /// Represents an aggregate's state has changed
    /// </summary>
    protected ChangeEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
 
    /// <summary>
    /// Set the sequence number
    /// </summary>
    /// <param name="number"></param>
    public void SetSequence(EventSequenceNumber number)
    {
        EventSequenceNumber = number;
    }
}
using Newtonsoft.Json;

namespace Truss.Application.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an aggregate's state has changed
/// </summary>
public abstract record ChangeEvent : IChangeEvent
{
    /// <summary>
    /// For general event versioning
    /// </summary>
    public const int ChangeEventVersion = 1;
            
    /// <summary>
    /// The unique id of the event
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();   
    
    /// <summary>
    /// Represents the unique identifier of an aggregate.
    /// </summary>
    public Guid AggregateId { get; }

    /// <summary>
    /// When the event occured
    /// </summary>
    public DateTime Time { get; private set; } = DateTime.UtcNow;
    
    /// <summary>
    /// The order of the event in an aggregate's event sequence
    /// </summary>
    [JsonProperty]
    public EventSequenceNumber? SequenceNumber { get; private set; }

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
        SequenceNumber = number;
    }
}
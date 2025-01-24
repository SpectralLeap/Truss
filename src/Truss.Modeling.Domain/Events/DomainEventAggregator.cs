namespace Truss.Modeling.Domain.Events;

/// <summary>
/// A collection of the events that occurred on an entity
/// </summary>
internal sealed class DomainEventAggregator
{
    private readonly HashSet<IDomainEvent> _domainEvents = new();
    
    /// <summary>
    /// Get all the events
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Events => _domainEvents.ToArray();

    /// <summary>
    /// Add a new event
    /// </summary>
    /// <param name="event"></param>
    public void Add(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    /// <summary>
    /// Clear all the events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
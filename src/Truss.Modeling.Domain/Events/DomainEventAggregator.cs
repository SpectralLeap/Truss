namespace Truss.Modeling.Domain.Events;

/// <summary>
/// A collection of the events that occurred on an entity
/// </summary>
internal sealed class DomainEventAggregator
{
    private readonly Dictionary<Guid, IDomainEvent> _domainEvents = new();
    
    /// <summary>
    /// Get all the events
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Events => _domainEvents.Values.ToList().AsReadOnly();

    /// <summary>
    /// Add a new event
    /// </summary>
    /// <param name="event"></param>
    public void Add(IDomainEvent @event)
    {
        var id = @event.Id;
        if (_domainEvents.ContainsKey(id)) return;
        _domainEvents.Add(id, @event);
    }

    /// <summary>
    /// Clear all the events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
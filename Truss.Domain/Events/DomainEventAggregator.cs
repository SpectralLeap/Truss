using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Events;

/// <summary>
/// A collection of the events that occurred on an entity
/// </summary>
internal sealed class DomainEventAggregator : IDomainEventAggregator
{
    private readonly Dictionary<Guid, DomainEvent> _domainEvents = new();
    
    /// <summary>
    /// Get all the events
    /// </summary>
    public IReadOnlyCollection<DomainEvent> Events => _domainEvents.Values.ToList().AsReadOnly();

    /// <summary>
    /// Add a new event
    /// </summary>
    /// <param name="event"></param>
    public void Add(DomainEvent @event)
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
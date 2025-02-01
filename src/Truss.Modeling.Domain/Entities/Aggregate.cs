using System.Reflection;
using Truss.Modeling.Domain.Events;


namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// An aggregate per DDD
///
/// https://martinfowler.com/bliki/DDD_Aggregate.html
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class Aggregate<TId>
    : Entity<TId>, IAggregate<TId>
{
    // ReSharper disable once StaticMemberInGenericType -- This is appropriate because each concrete type will have its own registry
    private static readonly EventHandlerRegistry _eventHandlerRegistry = new();

    /// <inheritdoc/>>
    public IReadOnlyCollection<IDomainEvent> PendingEvents => _pendingEvents;

    /// <inheritdoc/>>
    // ReSharper disable once MemberCanBePrivate.Global -- Version is used by the event store
    public long Version { get; set; }

    private readonly List<IDomainEvent> _pendingEvents = [];

    /// <summary>
    /// Requires an Id for consistency with the underlying event systems
    /// </summary>
    protected Aggregate()
    {
        if (_eventHandlerRegistry.AggregateIsRegistered(this)) return;

        _eventHandlerRegistry.Register(this);
    }

    /// <inheritdoc/>>
    public void ClearPendingEvents()
    {
        _pendingEvents.Clear();
    }


    /// <summary>
    /// Applies the event to the model and registers the event to be dispatched
    /// </summary>
    /// <param name="event"></param>
    protected void ApplyAndAddPendingEvent(
        IDomainEvent @event
    )
    {
        Apply(@event);

        _pendingEvents.Add(@event);
    }

    /// <summary>
    /// Applies the registered event handler
    /// </summary>
    /// <param name="event"></param>
    private void Apply(IDomainEvent @event)
    {
        _eventHandlerRegistry.Handle(this, @event);

        Version++;
    }
}
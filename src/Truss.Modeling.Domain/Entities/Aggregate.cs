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
    private static readonly EventHandlerRegistry EventHandlerRegistry = new();
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
        if (EventHandlerRegistry.AggregateIsRegistered(this)) return;

        EventHandlerRegistry.Register(this);
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
    /// <typeparam name="TDomainEvent"></typeparam>
    private void Apply<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : IDomainEvent
    {
        EventHandlerRegistry.Handle(this, @event);

        Version++;
    }
}
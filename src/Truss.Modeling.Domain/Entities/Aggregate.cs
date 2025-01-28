using System.Reflection;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;


namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// An aggregate per DDD
///
/// https://martinfowler.com/bliki/DDD_Aggregate.html
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class Aggregate<TId>
    : Entity<TId>, IAggregateRoot<TId>
{
    /// <inheritdoc/>>
    public IReadOnlyCollection<IDomainEvent> PendingEvents => _pendingEvents;

    /// <inheritdoc/>>
    // ReSharper disable once MemberCanBePrivate.Global -- Version is used by the event store
    public long Version { get; set; }

    private readonly List<IDomainEvent> _pendingEvents = [];

    /// <summary>
    /// A registry for derived class's event handlers
    /// </summary>
    private readonly IDomainEventHandlerRegistry _eventHandlerRegistry;

    /// <summary>
    /// Requires an Id for consistency with the underlying event systems
    /// </summary>
    /// <param name="id"></param>
    protected Aggregate(TId id)
        : base(id)
    {
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        Id = id;

        var type = GetType();

        _eventHandlerRegistry = new EventHandlerRegistry();

        // Register all methods named Apply that take a single parameter of type <see cref="IDomainEvent"/>
        // and add them to the handler registry
        var methods = type.GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        var handlerMethods = methods
            .Where(method => method.Name == "Apply"
                             && method.GetParameters().Length == 1
                             && typeof(IDomainEvent).IsAssignableFrom(
                                 method.GetParameters()[0].ParameterType
                             )
            )
            .Select(method =>
                new {
                    EventType = method.GetParameters()[0].ParameterType,
                    Method = method
                })
            .ToArray();

        foreach (var handlerMethod in handlerMethods)
        {
            _eventHandlerRegistry.AddHandler(
                handlerMethod.EventType,
                e => handlerMethod.Method.Invoke(this, [e])
            );
        }
    }

    /// <inheritdoc/>>
    public void ClearPendingEvents()
    {
        _pendingEvents.Clear();
    }


    /// <summary>
    /// Registers an event to be dispatched
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
    /// Applies the registered change event handler
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="TDomainEvent"></typeparam>
    private void Apply<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : IDomainEvent
    {
        _eventHandlerRegistry.Handle(@event);

        Version++;
    }
}
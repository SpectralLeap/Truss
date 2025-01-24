using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Builds an Event Sourced Context provider
/// </summary>
internal sealed class EventHandlerRegistry
    : IDomainEventHandlerRegistry
{
    private readonly Dictionary<Type, Action<IDomainEvent>> _domainEventHandlers = new();

    /// <inheritdoc />
    public void AddHandler(
        Type eventType,
        Action<IDomainEvent> handler
    )
    {
        _domainEventHandlers[eventType] = handler;
    }

    /// <summary>
    /// Calls the handler
    /// </summary>
    /// <returns></returns>
    public void Handle(
        IDomainEvent @event
    )
    {
        var type = @event.GetType();

        _domainEventHandlers.TryGetValue(type, out var handler);

        handler?.Invoke(@event);
    }
}
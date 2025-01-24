using Truss.Modeling.Domain.Events;
using Truss.Monads.Results;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Registration container for handlers of aggregate change events
/// </summary>
public interface IDomainEventHandlerRegistry
{
    /// <summary>
    /// <b>The changes should already be validated. No business rule validations should occur in registered handlers.</b>
    /// <br/>
    /// <br/>
    /// Register a handler to apply and event.
    /// </summary>
    /// <param name="eventType">
    /// The <see cref="Type"/> of the <see cref="IDomainEvent"/> to handle
    /// </param>
    /// <param name="handler">
    /// A method taking a <see cref="IDomainEvent"/> and returning a <see cref="Result"/>
    /// </param>
    public void AddHandler(
        Type eventType,
        Action<IDomainEvent> handler
    );

    /// <summary>
    /// Calls the handler
    /// </summary>
    /// Apply the handler for the <see cref="IDomainEvent"/>
    public void Handle(
        IDomainEvent @event
    );
}
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Events;

/// <summary>
/// Dispatches domain events to the event bus
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatch and clear the events from an aggregate
    /// </summary>
    /// <param name="withEvents"></param>
    /// <param name="cancellationToken"></param>
    Task DispatchAndClearDomainEvents(
        IAggregate withEvents,
        CancellationToken cancellationToken
    );
}
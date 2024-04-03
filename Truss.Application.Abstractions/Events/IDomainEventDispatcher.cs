using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Abstractions.Events;

/// <summary>
/// Dispatches domain events to the event bus
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatch and clear the events from an aggregate
    /// </summary>
    /// <param name="rootWithEvents"></param>
    /// <param name="cancellationToken"></param>
    Task DispatchAndClearDomainEvents(IAggregateRoot rootWithEvents, CancellationToken cancellationToken);

    /// <summary>
    ///  Dispatch and clear the events from a set of aggregates
    /// </summary>
    /// <param name="rootsWithEvents"></param>
    /// <param name="cancellationToken"></param>
    Task DispatchAndClearDomainEvents(IEnumerable<IAggregateRoot> rootsWithEvents, CancellationToken cancellationToken);
}
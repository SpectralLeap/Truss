using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.Events;

namespace Truss.ConcreteServices;

/// <summary>
/// Generally used to dispatch events stored on an
/// aggregate after changes have been made.
/// </summary>
internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IDomainEventBus _domainEventBus;

    /// <summary>
    /// Uses the event bus to dispatch the events
    /// </summary>
    /// <param name="domainEventBus"></param>
    public DomainEventDispatcher(
        IDomainEventBus domainEventBus
    )
    {
        _domainEventBus = domainEventBus;
    }

    /// <summary>
    /// Dispatch and clear the events from an aggregate
    /// </summary>
    /// <param name="rootWithEvents"></param>
    /// <param name="cancellationToken"></param>
    public async Task DispatchAndClearDomainEvents(IAggregateRoot rootWithEvents, CancellationToken cancellationToken)
    {
        await DispatchAndClearDomainEvents(
            [rootWithEvents],
            cancellationToken
        ).ConfigureAwait(false);
    }
    
    /// <summary>
    ///  Dispatch and clear the events from a set of aggregates
    /// </summary>
    /// <param name="rootsWithEvents"></param>
    /// <param name="cancellationToken"></param>
    private async Task DispatchAndClearDomainEvents(
        IReadOnlyCollection<IAggregateRoot> rootsWithEvents,
        CancellationToken cancellationToken
    )
    {
        foreach (var root in rootsWithEvents)
        {
            foreach (var @event in root.PendingEvents)
            {
                await _domainEventBus
                    .Publish(
                        @event,
                        cancellationToken
                    ).ConfigureAwait(false);
            }

            root.ClearPendingEvents();
        }
    }
}
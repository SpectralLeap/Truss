using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.DomainEvents;

/// <summary>
/// Generally used to dispatch events stored on an
/// aggregate after changes have been made.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IDomainEventBus _domainEventBus;
    private readonly IChangeEventBus _changeEventBus;

    /// <summary>
    /// Uses the event bus to dispatch the events
    /// </summary>
    /// <param name="domainEventBus"></param>
    public DomainEventDispatcher(
        IDomainEventBus domainEventBus,
        IChangeEventBus changeEventBus
    )
    {
        _domainEventBus = domainEventBus;
        _changeEventBus = changeEventBus;
    }

    /// <summary>
    /// Dispatch and clear the events from an aggregate
    /// </summary>
    /// <param name="rootWithEvents"></param>
    /// <param name="cancellationToken"></param>
    public async Task DispatchAndClearDomainEvents(IAggregateRoot rootWithEvents, CancellationToken cancellationToken)
    {
        await DispatchAndClearDomainEvents(new [] {rootWithEvents}, cancellationToken).ConfigureAwait(false);
    }
    
    /// <summary>
    ///  Dispatch and clear the events from a set of aggregates
    /// </summary>
    /// <param name="rootsWithEvents"></param>
    /// <param name="cancellationToken"></param>
    private async Task DispatchAndClearDomainEvents(IEnumerable<IAggregateRoot> rootsWithEvents, CancellationToken cancellationToken)
    {
        foreach (var root in rootsWithEvents)
        {
            var events = root.DomainEvents().ToArray();
            
            foreach (var @event in events)
            {
                await _domainEventBus.Publish(@event, cancellationToken).ConfigureAwait(false);

                if (@event is ChangeEvent changeEvent)
                {
                    await _changeEventBus.Publish(changeEvent, cancellationToken);
                }
            }

            root.ClearEvents();
        }
    }
}
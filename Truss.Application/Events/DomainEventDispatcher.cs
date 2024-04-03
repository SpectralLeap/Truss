using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.Events;

namespace Truss.Application.Events;

/// <summary>
/// Generally used to dispatch events stored on an
/// aggregate after changes have been made.
/// </summary>
internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IEventBus _eventBus;

    /// <summary>
    /// Uses the event bus to dispatch the events
    /// </summary>
    /// <param name="eventBus"></param>
    public DomainEventDispatcher(IEventBus eventBus)
    {
        _eventBus = eventBus;
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
            var events = root.Events().ToArray();
            
            root.ClearEvents();

            foreach (var @event in events)
            {
                await _eventBus.Publish(@event, cancellationToken).ConfigureAwait(false);
            }

        }
    }
}
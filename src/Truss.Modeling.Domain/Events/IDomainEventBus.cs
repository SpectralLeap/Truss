namespace Truss.Modeling.Domain.Events;

/// <summary>
/// For transmitting domain events
/// </summary>
public interface IDomainEventBus
{
    /// <summary>
    /// Publishes the event to the bus
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDomainEvent"></typeparam>
    /// <returns></returns>
    Task Publish<TDomainEvent>(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken
    ) where TDomainEvent : IDomainEvent;
}
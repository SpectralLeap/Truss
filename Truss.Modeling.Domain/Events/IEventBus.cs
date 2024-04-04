namespace Truss.Modeling.Domain.Events;

/// <summary>
/// For transmitting domain events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes the event to the bus
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : DomainEvent;
}
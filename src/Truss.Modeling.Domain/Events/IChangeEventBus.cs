using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Domain.Events;

/// <summary>
/// For transmitting change events
/// </summary>
public interface IChangeEventBus
{
    /// <summary>
    /// Publishes the event to the bus
    /// </summary>
    /// <param name="changeEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TChangeEvent"></typeparam>
    /// <returns></returns>
    Task Publish<TChangeEvent>(TChangeEvent changeEvent, CancellationToken cancellationToken) where TChangeEvent : ChangeEvent;
}
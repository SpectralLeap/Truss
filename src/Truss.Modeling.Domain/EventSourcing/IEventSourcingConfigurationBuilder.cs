namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Builds the configuration set for an aggregate root
/// </summary>
/// <typeparam name="TRoot"></typeparam>
public interface IEventSourcingConfigurationBuilder<TRoot>
{
    /// <summary>
    /// Registration container for handlers of aggregate change events
    /// </summary>
    public IChangeEventHandlerRegistry<TRoot> Handlers { get; }
}
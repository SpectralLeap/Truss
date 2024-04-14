namespace Truss.Modeling.Domain.EventSourcing;

internal sealed class EventSourcingConfigurationBuilder<TRoot> : IEventSourcingConfigurationBuilder<TRoot>
{
    public IChangeEventHandlerRegistry<TRoot> Handlers { get; }

    public EventSourcingConfigurationBuilder(IChangeEventHandlerRegistry<TRoot> handlerRegistry)
    {
        Handlers = handlerRegistry;
    }
}
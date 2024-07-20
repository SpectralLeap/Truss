using Truss.Monads.Results;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Registration container for handlers of aggregate change events
/// </summary>
/// <typeparam name="TRoot"></typeparam>
public interface IChangeEventHandlerRegistry<TRoot>
{
    /// <summary>
    /// <b>The changes should already be validated. No business rule validations should occur in registered handlers.</b>
    /// <br/>
    /// <br/>
    /// Register a handler to apply changes when an event has been validated.
    /// </summary>
    /// <param name="handler">A method taking a <see cref="ChangeEvent"/> and returning a <see cref="Result"/></param>
    /// <typeparam name="TChangeEvent">The <see cref="Type"/> of the <see cref="ChangeEvent"/></typeparam>
    public IChangeEventHandlerRegistry<TRoot> AddHandler<TChangeEvent>(Func<TChangeEvent, Result<TRoot>> handler)
        where TChangeEvent : ChangeEvent;

    /// <summary>
    /// Calls the handler
    /// </summary>
    /// <typeparam name="TChangeEvent"></typeparam>
    /// <returns></returns>
    public Result<TRoot> Handle<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent;
}
using Truss.Monads.Results;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Builds an Event Sourced Context provider
/// </summary>
internal sealed class ChangeEventHandlerRegistry<TRoot, TId> : IChangeEventHandlerRegistry<TRoot> 
    where TRoot : EventSourcedAggregateRoot<TRoot, TId>
{
    private readonly Dictionary<Type, Func<ChangeEvent, Result<TRoot>>> _changeEventHandlers = new();

    /// <inheritdoc />
    public IChangeEventHandlerRegistry<TRoot> AddHandler<TChangeEvent>(Func<TChangeEvent, Result<TRoot>> handler) where TChangeEvent : ChangeEvent
    {
        _changeEventHandlers[typeof(TChangeEvent)] = @event => handler((TChangeEvent) @event);
        
        return this;
    }

    /// <summary>
    /// Calls the handler
    /// </summary>
    /// <typeparam name="TChangeEvent"></typeparam>
    /// <returns></returns>
    public Result<TRoot> Handle<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        var type = @event.GetType();
        
        if (!_changeEventHandlers.TryGetValue(type, out var handler))
        {
            return Result.Fail($"The event type {type.Name} is not registered");
        }
        
        return handler(@event);
    }
    
    
}
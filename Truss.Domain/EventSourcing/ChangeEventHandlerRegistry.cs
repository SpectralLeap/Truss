using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.EventSourcing.Writing;
using Truss.Results;

namespace Truss.Domain.EventSourcing;

/// <summary>
/// Builds an Event Sourced Context provider
/// </summary>
internal sealed class ChangeEventHandlerRegistry<TRoot, TId> : IChangeEventHandlerRegistry<TRoot> where TId : AggregateRootId<Guid>
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
    internal Result<TRoot> Handle<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        return _changeEventHandlers[@event.GetType()](@event);
    }
    
    
}
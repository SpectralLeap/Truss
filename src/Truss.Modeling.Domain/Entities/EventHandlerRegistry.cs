using System.Collections.Concurrent;
using System.Reflection;
using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// Builds an Event Sourced Context provider
/// </summary>
internal sealed class EventHandlerRegistry
{
    private readonly ConcurrentDictionary<Type, Dictionary<Type, Action<IAggregate, IDomainEvent>>> _domainEventHandlers = new();

    public bool AggregateIsRegistered(
        IAggregate aggregate
    )
    {
        return _domainEventHandlers.ContainsKey(aggregate.GetType());
    }


    /// <summary>
    /// Calls the handler
    /// </summary>
    /// <returns></returns>
    public void Handle(
        IAggregate aggregate,
        IDomainEvent @event
    )
    {
        var aggregateType = aggregate.GetType();
        var eventType = @event.GetType();

        _domainEventHandlers.TryGetValue(
            aggregateType,
            out var handlers
        );

        var handler = handlers?
            .GetValueOrDefault(eventType);

        handler?.Invoke(aggregate, @event);
    }

    /// <summary>
    /// Registers an aggregate's event handlers
    /// </summary>
    /// <param name="aggregate">
    /// The aggregate to register
    /// </param>
    public void Register(IAggregate aggregate)
    {
        var type = aggregate.GetType();

        // Register all methods named Apply that take a single parameter of type <see cref="IDomainEvent"/>
        // and add them to the handler registry
        var methods = type.GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        var handlerMethods = methods
            .Where(method => method.Name == "Apply"
                             && method.GetParameters().Length == 1
                             && typeof(IDomainEvent).IsAssignableFrom(
                                 method.GetParameters()[0].ParameterType
                             )
            )
            .Select(method =>
                new {
                    EventType = method.GetParameters()[0].ParameterType,
                    Method = method
                })
            .ToArray();

        var handlers = new Dictionary<Type, Action<IAggregate, IDomainEvent>>();

        foreach (var handlerMethod in handlerMethods)
        {
            handlers.Add(
                handlerMethod.EventType,
                (a, e) => handlerMethod.Method.Invoke(a, [e])
            );
        }

        _domainEventHandlers.TryAdd(type, handlers);
    }
}
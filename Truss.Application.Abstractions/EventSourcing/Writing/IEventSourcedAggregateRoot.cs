using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Abstractions.EventSourcing.Writing;

/// <summary>
/// An aggregate root that stores event sourcing specific events
/// </summary>
public interface IEventSourcedAggregateRoot : IAggregateRoot
{
    /// <summary>
    /// Change events pending storage
    /// </summary>
    public IReadOnlyCollection<ChangeEvent> PendingChangeEvents { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEventSourcedAggregateRoot<out TId> : IEventSourcedAggregateRoot
    where TId : AggregateRootId<Guid>
{
    /// <summary>
    /// The event sourced aggregate root's Id
    /// </summary>
    public TId Id { get; }
}
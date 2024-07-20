using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.EventSourcing;


/// <summary>
/// 
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEventSourcedAggregateRoot<out TId>
    where TId : AggregateRootId<Guid>
{
    /// <summary>
    /// The event sourced aggregate root's Id
    /// </summary>
    public TId Id { get; }
    
    /// <summary>
    /// Change events pending storage
    /// </summary>
    public IReadOnlyCollection<ChangeEvent> PendingChangeEvents { get; }
}
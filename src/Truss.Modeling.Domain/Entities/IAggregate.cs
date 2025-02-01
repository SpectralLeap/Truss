using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// And aggregate root per DDD
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IAggregate<out TId> : IAggregate
{
    /// <summary>
    /// The unique identifier of the aggregate
    /// </summary>
    public TId Id { get; }
}


/// <summary>
/// And aggregate root per DDD
/// </summary>
public interface IAggregate
{
    /// <summary>
    /// The version of the aggregate
    /// </summary>
    public long Version { get; }

    /// <summary>
    /// The Aggregate's uncommitted domain events (deltas)
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> PendingEvents { get; }

    /// <summary>
    /// Clear the aggregate's pending domain events
    /// </summary>
    public void ClearPendingEvents();
}
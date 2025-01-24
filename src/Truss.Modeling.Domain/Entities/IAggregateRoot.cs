using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// And aggregate root per DDD
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IAggregateRoot<out TId> : IAggregateRoot
{
    /// <summary>
    /// The unique identifier of the aggregate
    /// </summary>
    public TId Id { get; }
}


/// <summary>
/// And aggregate root per DDD
/// </summary>
public interface IAggregateRoot
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
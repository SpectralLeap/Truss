using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Marker interface to represent a change event for dependency resolution
/// </summary>
public interface IChangeEvent : IDomainEvent
{
    /// <summary>
    /// Represents the unique identifier of an aggregate.
    /// </summary>
    Guid AggregateId { get; }
}
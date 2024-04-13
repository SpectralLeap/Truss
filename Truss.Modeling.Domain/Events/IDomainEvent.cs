namespace Truss.Modeling.Domain.Events;

/// <summary>
/// Marker interface to represent a domain event
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// The unique id of the event
    /// </summary>
    public Guid Id { get; }
}
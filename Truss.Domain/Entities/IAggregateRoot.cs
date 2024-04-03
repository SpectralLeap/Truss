namespace Truss.Application.Abstractions.Domain;

/// <summary>
/// An aggregate root per DDD
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// The domain events stored on the aggregate
    /// </summary>
    public IReadOnlyCollection<DomainEvent> Events();
    
    /// <summary>
    /// Clear the aggregate's domain events
    /// </summary>
    public void ClearEvents();
}
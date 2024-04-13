namespace Truss.Modeling.Domain.Events;

/// <summary>
/// Captures an event from the domain that can
/// be propagated outward
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// For general event versioning
    /// </summary>
    public const int DomainEventVersion = 1;
        
    /// <inheritdoc />
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    /// <summary>
    /// When the event occured
    /// </summary>
    public DateTime Time { get; private set; } = DateTime.UtcNow;
}
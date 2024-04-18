using Truss.Modeling.Domain.Events;


namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// An aggregate root per DDD
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TIdType"></typeparam>
public abstract class AggregateRoot<TId, TIdType> 
    : Entity<TId>,
        IAggregateRoot where TId : AggregateRootId<TIdType>
{
   
    /// <inheritdoc/>>
    public IReadOnlyCollection<IDomainEvent> DomainEvents() => _domainEventAggregator.Events;
    private readonly DomainEventAggregator _domainEventAggregator = new();

    /// <inheritdoc/>>
    public void ClearEvents()
    {
        _domainEventAggregator.ClearDomainEvents();    
    }

 
    /// <summary>
    /// Requires an Id for consistency with the underlying event systems
    /// </summary>
    /// <param name="id"></param>
    protected AggregateRoot(TId id) : base(id)
    {
    }
    
    /// <summary>
    /// Registers an event for later disbursement
    /// </summary>
    /// <param name="event"></param>
    protected void RegisterDomainEvent(IDomainEvent @event)
    {
        _domainEventAggregator.Add(@event);     
    }
}
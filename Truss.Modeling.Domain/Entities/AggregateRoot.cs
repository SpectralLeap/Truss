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
    public IReadOnlyCollection<IDomainEvent> Events() => _domainEventAggregator.Events;

    /// <inheritdoc/>>
    public void ClearEvents()
    {
        _domainEventAggregator.ClearDomainEvents();    
    }

    private readonly IDomainEventAggregator _domainEventAggregator;
 
    /// <summary>
    /// Requires an Id for consistency with the underlying event systems
    /// </summary>
    /// <param name="id"></param>
    protected AggregateRoot(TId id) : this(id, new DomainEventAggregator())
    {
    }

    private AggregateRoot(TId id, IDomainEventAggregator eventAggregator) : base(id)
    {
        _domainEventAggregator = eventAggregator;
    }
    
    /// <summary>
    /// Registers an event for later disbursement
    /// </summary>
    /// <param name="event"></param>
    protected void RegisterDomainEvent(DomainEvent @event)
    {
        _domainEventAggregator.Add(@event);     
    }
}
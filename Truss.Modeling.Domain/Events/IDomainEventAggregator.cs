namespace Truss.Modeling.Domain.Events;

internal interface IDomainEventAggregator
{
    IReadOnlyCollection<DomainEvent> Events { get; }
    void Add(DomainEvent @event);
    void ClearDomainEvents();
}
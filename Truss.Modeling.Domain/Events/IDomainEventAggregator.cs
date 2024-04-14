namespace Truss.Modeling.Domain.Events;

internal interface IDomainEventAggregator
{
    IReadOnlyCollection<IDomainEvent> Events { get; }
    void Add(IDomainEvent @event);
    void ClearDomainEvents();
}
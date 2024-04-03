using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Events;

internal interface IDomainEventAggregator
{
    IReadOnlyCollection<DomainEvent> Events { get; }
    void Add(DomainEvent @event);
    void ClearDomainEvents();
}
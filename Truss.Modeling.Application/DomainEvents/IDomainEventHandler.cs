using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Application.DomainEvents;

/// <summary>
/// For receiving and acting on domain events
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IDomainEventHandler<in TDomainEvent> 
    where TDomainEvent : IDomainEvent
{
    public Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}

using MediatR;
using Truss.Domain.Events;

namespace Truss.Application.Cqrs.EventSourcing.Events;

/// <summary>
/// For receiving and acting on domain events
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IDomainEventHandler<in TDomainEvent> 
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}

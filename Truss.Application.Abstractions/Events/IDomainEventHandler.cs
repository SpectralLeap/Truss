using MediatR;
using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Abstractions.Events;

/// <summary>
/// For receiving and acting on domain events
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IDomainEventHandler<in TDomainEvent> 
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}

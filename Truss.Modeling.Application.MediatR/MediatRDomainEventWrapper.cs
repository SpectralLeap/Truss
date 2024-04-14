using MediatR;
using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRDomainEventWrapper<TDomainEvent>(TDomainEvent domainEvent) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}
using MediatR;
using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Infrastructure.MediatR.Wrappers;

internal sealed class DomainEventWrapper<TDomainEvent>(TDomainEvent domainEvent) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}
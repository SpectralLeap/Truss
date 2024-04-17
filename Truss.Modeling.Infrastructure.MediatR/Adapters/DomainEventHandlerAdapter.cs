using MediatR;
using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;

namespace Truss.Modeling.Infrastructure.MediatR.Adapters;

internal sealed class DomainEventHandlerAdapter<TDomainEvent>
    : INotificationHandler<DomainEventWrapper<TDomainEvent>> 
    where TDomainEvent : DomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> _internalHandler;

    public DomainEventHandlerAdapter(IDomainEventHandler<TDomainEvent> internalHandler)
    {
        _internalHandler = internalHandler;
    }
    
    public async Task Handle(DomainEventWrapper<TDomainEvent> domainEventWrapper, CancellationToken cancellationToken)
    {
        await _internalHandler.Handle(domainEventWrapper.DomainEvent, cancellationToken);
    }
}
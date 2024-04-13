using MediatR;
using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.Events;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRWrappedDomainEventHandlerAdapter<TDomainEvent>
    : INotificationHandler<MediatRDomainEventWrapper<TDomainEvent>> 
    where TDomainEvent : DomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> _internalHandler;

    public MediatRWrappedDomainEventHandlerAdapter(IDomainEventHandler<TDomainEvent> internalHandler)
    {
        _internalHandler = internalHandler;
    }
    
    public async Task Handle(MediatRDomainEventWrapper<TDomainEvent> domainEventWrapper, CancellationToken cancellationToken)
    {
        await _internalHandler.Handle(domainEventWrapper.DomainEvent, cancellationToken);
    }
}
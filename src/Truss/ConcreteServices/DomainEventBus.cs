using MediatR;
using Truss.Modeling.Domain.Events;

namespace Truss.ConcreteServices;

internal sealed class DomainEventBus : IDomainEventBus
{
    private readonly IMediator _mediator;

    public DomainEventBus(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Publish<TDomainEvent>(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken
    ) where TDomainEvent : IDomainEvent
    {
        await _mediator.Publish(domainEvent, cancellationToken);
    }
}
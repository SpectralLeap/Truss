using MediatR;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;

namespace Truss.Modeling.Infrastructure.MediatR.Buses;

/// <summary>
/// A concrete event bus that sends events
/// </summary>
internal sealed class DomainEventBus : IDomainEventBus
{
    private readonly IMediator _mediator;

    public DomainEventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken) 
        where TDomainEvent : IDomainEvent
    {
        var wrappedEvent = Activator.CreateInstance(
            typeof(DomainEventWrapper<>).MakeGenericType(
                domainEvent.GetType()), domainEvent);
        
        await _mediator.Publish(wrappedEvent, cancellationToken).ConfigureAwait(false);
    }
}
using MediatR;
using Truss.Application.Abstractions.Domain;
using Truss.Application.Abstractions.Events;

namespace Truss.Application.Events;

/// <summary>
/// A concrete event bus that sends events
/// </summary>
internal sealed class EventBus : IEventBus
{
    private readonly IMediator _mediator;

    public EventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) 
        where TEvent : DomainEvent
    {
        await _mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }
}
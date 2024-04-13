using MediatR;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.MediatR;

/// <summary>
/// A concrete event bus that sends events
/// </summary>
internal sealed class ChangeEventBus : IChangeEventBus
{
    private readonly IMediator _mediator;

    public ChangeEventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish<TChangeEvent>(TChangeEvent changeEvent, CancellationToken cancellationToken) 
        where TChangeEvent : ChangeEvent
    {
        var wrappedEvent = new MediatRChangeEventWrapper<TChangeEvent>(changeEvent);
        
        await _mediator.Publish(wrappedEvent, cancellationToken).ConfigureAwait(false);
    }
}
using MediatR;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;

namespace Truss.Modeling.Infrastructure.MediatR.Buses;

/// <summary>
/// A concrete event bus that sends change events
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
        var wrappedEvent = new ChangeEventWrapper<TChangeEvent>(changeEvent);
        
        await _mediator.Publish(wrappedEvent, cancellationToken).ConfigureAwait(false);
    }
}
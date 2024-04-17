using MediatR;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;

namespace Truss.Modeling.Infrastructure.MediatR.Adapters;

internal sealed class ChangeEventHandlerAdapter<TChangeEvent> 
    : INotificationHandler<ChangeEventWrapper<TChangeEvent>>
    where TChangeEvent : ChangeEvent
{
    private readonly IChangeEventHandler<TChangeEvent> _internalHandler;

    public ChangeEventHandlerAdapter(IChangeEventHandler<TChangeEvent> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task Handle(ChangeEventWrapper<TChangeEvent> changeEventWrapper, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(changeEventWrapper.ChangeEvent, cancellationToken);
    }
}
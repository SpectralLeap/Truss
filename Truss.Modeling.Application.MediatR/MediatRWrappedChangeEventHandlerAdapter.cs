using MediatR;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Domain.EventSourcing;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRWrappedChangeEventHandlerAdapter<TChangeEvent> 
    : INotificationHandler<MediatRChangeEventWrapper<TChangeEvent>>
    where TChangeEvent : ChangeEvent
{
    private readonly IChangeEventHandler<TChangeEvent> _internalHandler;

    public MediatRWrappedChangeEventHandlerAdapter(IChangeEventHandler<TChangeEvent> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task Handle(MediatRChangeEventWrapper<TChangeEvent> changeEventWrapper, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(changeEventWrapper.ChangeEvent, cancellationToken);
    }
}
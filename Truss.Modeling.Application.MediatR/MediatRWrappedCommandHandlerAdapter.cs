using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;


internal sealed class MediatRWrappedCommandHandlerAdapter<TCommand> 
    : IRequestHandler<MediatRCommandWrapper<TCommand>, Result<Nil>>
    where TCommand : Command
{
    private readonly ICommandHandler<TCommand> _internalHandler;

    public MediatRWrappedCommandHandlerAdapter(ICommandHandler<TCommand> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public async Task<Result<Nil>> Handle(MediatRCommandWrapper<TCommand> request, CancellationToken cancellationToken)
    {
        return await _internalHandler.Handle(request.Command, cancellationToken);
    }
}

internal sealed class MediatRWrappedCommandHandlerAdapter<TCommand, TResult> 
    : IRequestHandler<MediatRCommandWrapper<TCommand, TResult>, Result<TResult>>
    where TCommand : Command<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _internalHandler;

    public MediatRWrappedCommandHandlerAdapter(ICommandHandler<TCommand, TResult> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task<Result<TResult>> Handle(MediatRCommandWrapper<TCommand, TResult> request, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(request.Command, cancellationToken);
    }
}
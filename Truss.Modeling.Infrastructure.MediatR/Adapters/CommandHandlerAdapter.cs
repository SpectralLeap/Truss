using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.MediatR.Adapters;


internal sealed class CommandHandlerAdapter<TCommand> 
    : IRequestHandler<CommandWrapper<TCommand>, Result<Nil>>
    where TCommand : Command
{
    private readonly ICommandHandler<TCommand> _internalHandler;

    public CommandHandlerAdapter(ICommandHandler<TCommand> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public async Task<Result<Nil>> Handle(CommandWrapper<TCommand> request, CancellationToken cancellationToken)
    {
        return await _internalHandler.Handle(request.Command, cancellationToken);
    }
}

internal sealed class CommandHandlerAdapter<TCommand, TResult> 
    : IRequestHandler<CommandWrapper<TCommand, TResult>, Result<TResult>>
    where TCommand : Command<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _internalHandler;

    public CommandHandlerAdapter(ICommandHandler<TCommand, TResult> internalHandler)
    {
        _internalHandler = internalHandler;
    }

    public Task<Result<TResult>> Handle(CommandWrapper<TCommand, TResult> request, CancellationToken cancellationToken)
    {
        return _internalHandler.Handle(request.Command, cancellationToken);
    }
}
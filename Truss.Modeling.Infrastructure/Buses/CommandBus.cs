using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.Buses;

public sealed class CommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    public CommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<Result<Nil>> SendCommand<TCommand>(
        TCommand command,
        CancellationToken cancellationToken
    ) where TCommand : Command
    {
        return await _mediator.Send(command, cancellationToken);
    }

    public async Task<Result<TResult>> SendCommand<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken
    ) where TCommand : Command<TResult>
    {
        return await _mediator.Send(command, cancellationToken);
    }
}
using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.DefaultServices.Buses;

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
    ) where TCommand : ICommand
    {
        return await _mediator.Send(command, cancellationToken);
    }

    public async Task<Result<TResult>> SendCommand<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken
    ) where TCommand : ICommand<TResult>
    {
        return await _mediator.Send(command, cancellationToken);
    }
}
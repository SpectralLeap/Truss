using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.ConcreteServices;

internal sealed class CommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    public CommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<Result<Nil>> SendCommand(
        ICommand command,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(command, cancellationToken);
    }

    public async Task<Result<TResult>> SendCommand<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(command, cancellationToken);
    }
}
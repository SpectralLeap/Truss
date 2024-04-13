using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;

/// <summary>
/// Concrete command bus for delivering commands
/// </summary>
internal sealed class CommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Uses a mediator as the bus
    /// </summary>
    /// <param name="mediator"></param>
    public CommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Send a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public async Task<Result<Nil>> SendCommand<TCommand>(TCommand command) 
        where TCommand : Command
    {
        var wrappedCommand = new MediatRCommandWrapper<TCommand>(command);
        
        var result = await _mediator.Send(wrappedCommand).ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Send a command with a nonstandard result to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public async Task<Result<TResult>> SendCommand<TCommand, TResult>(TCommand command) 
        where TCommand : Command<TResult>
    {
        var wrappedCommand = new MediatRCommandWrapper<TCommand, TResult>(command);
        
        var result = await _mediator.Send(wrappedCommand).ConfigureAwait(false);
        
        return result;
    }
    
}
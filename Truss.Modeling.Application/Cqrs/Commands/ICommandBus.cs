using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Commands;

/// <summary>
/// Delivers commands to the bus
/// </summary>
public interface ICommandBus
{
    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns><see cref="Result"/></returns>
    public Task<Result<Nil>> SendCommand<TCommand>(
        TCommand command,
        CancellationToken cancellationToken = new()
     ) where TCommand : Command;

    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A specific type of <see cref="Result"/></returns>
    public Task<Result<TResult>> SendCommand<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken = new()
    )
        where TCommand : Command<TResult>;
}
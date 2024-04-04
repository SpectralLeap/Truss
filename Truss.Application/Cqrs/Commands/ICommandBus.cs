using Truss.Results;

namespace Truss.Application.Cqrs.Commands;

/// <summary>
/// Delivers commands to the bus
/// </summary>
public interface ICommandBus
{
    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns><see cref="Result"/></returns>
     public Task<Result<None>> SendCommand<TCommand>(TCommand command) where TCommand : Command;

    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A specific type of <see cref="Result"/></returns>
    public Task<Result<TResult>> SendCommand<TCommand, TResult>(TCommand command)
        where TCommand : Command<TResult>;
}
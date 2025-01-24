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
    /// <param name="command">
    /// The command to dispatch
    /// </param>
    /// <param name="cancellationToken">
    /// Optional cancellation token
    /// </param>
    /// <returns><see cref="Result"/>An empty result</returns>
    public Task<Result<Nil>> SendCommand(
        ICommand command,
        CancellationToken cancellationToken = new()
     );

    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command">
    /// The command to dispatch
    /// </param>
    /// <param name="cancellationToken">
    /// Optional cancellation token
    /// </param>
    /// <typeparam name="TResult">
    /// The type of result to return
    /// </typeparam>
    /// <returns>A <see cref="Result"/> of <see cref="TResult"/></returns>
    public Task<Result<TResult>> SendCommand<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = new()
    );
}
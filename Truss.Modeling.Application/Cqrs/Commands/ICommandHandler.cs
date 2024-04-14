using Truss.Monads.Results;

#pragma warning disable CS0108, CS0114

namespace Truss.Modeling.Application.Cqrs.Commands;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand>
    where TCommand : Command
{
     /// <summary>
     /// Handle the command's execution
     /// </summary>
     /// <param name="command"></param>
     /// <param name="cancellationToken"></param>
     /// <returns></returns>
     public Task<Result<Nil>> Handle(TCommand command, CancellationToken cancellationToken);   
}

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<in TCommand, TResult> 
    where TCommand : Command<TResult>
{
    /// <summary>
    /// Handle the command's execution
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
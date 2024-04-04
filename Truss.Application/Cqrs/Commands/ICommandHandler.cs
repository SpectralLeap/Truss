using MediatR;
using Truss.Results;

#pragma warning disable CS0108, CS0114

namespace Truss.Application.Cqrs.Commands;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, Result<None>>
    where TCommand : Command;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<in TCommand, TResult> 
    : IRequestHandler<TCommand, Result<TResult>>
    where TCommand : Command<TResult>
{
    /// <summary>
    /// Handle the command's execution
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<TResult>> Handle(TCommand request, CancellationToken cancellationToken);
}
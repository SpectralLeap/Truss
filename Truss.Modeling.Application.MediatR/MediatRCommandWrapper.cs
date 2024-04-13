using MediatR;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;


internal sealed class MediatRCommandWrapper<TCommand>(TCommand command) : IRequest<Result<Nil>>
    where TCommand : Command
{
    public TCommand Command { get; } = command;
}

internal sealed class MediatRCommandWrapper<TCommand, TResult>(TCommand command) : IRequest<Result<TResult>>
    where TCommand : Command<TResult>
{
    public TCommand Command { get; } = command;
}
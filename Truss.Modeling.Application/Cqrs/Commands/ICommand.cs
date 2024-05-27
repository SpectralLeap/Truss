using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Commands;

public interface ICommand : IRequest<Result<Nil>>;

/// <summary>
/// Command for CQRS that returns a specific result type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ICommand<TResult> : IRequest<Result<TResult>>;
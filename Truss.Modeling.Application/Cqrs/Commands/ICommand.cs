using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Commands;

public interface ICommand : IRequest<Result<Nil>>;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
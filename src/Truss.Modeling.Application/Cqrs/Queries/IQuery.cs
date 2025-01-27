using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Queries;

/// <summary>
/// Query for CQRS
/// </summary>
/// <typeparam name="TResult">
///The type of the result
/// </typeparam>
public interface IQuery<TResult>
    : IRequest<Result<TResult>>;
using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Queries;

/// <summary>
/// Interface for a query
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IQuery<TResult> 
    : IRequest<Result<TResult>>;
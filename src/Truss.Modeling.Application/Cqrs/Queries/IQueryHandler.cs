// ReSharper disable UnusedMember.Global

using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Queries;

/// <summary>
/// Handles a query of the specified query type
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IQueryHandler<in TQuery, TResult>
    : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>
{
    
    /// <summary>
    /// Handle a query
    /// </summary>
    /// <param name="query">
    /// The query to handle
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token
    /// </param>
    /// <returns></returns>
    public new Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
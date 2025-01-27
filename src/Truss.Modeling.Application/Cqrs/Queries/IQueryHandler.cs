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
    /// Handles the query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public new Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
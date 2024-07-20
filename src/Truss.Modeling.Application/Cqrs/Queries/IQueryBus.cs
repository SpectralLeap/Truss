using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.Queries;

/// <summary>
/// Bus for sending queries into the mediator system
/// </summary>
public interface IQueryBus
{
    /// <summary>
    /// Bus a query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    /// <returns></returns>
    public Task<Result<TResult>> SendQuery<TQuery, TResult>(
        TQuery query,
        CancellationToken cancellationToken = new()
    )
        where TQuery : Query<TResult>;
}
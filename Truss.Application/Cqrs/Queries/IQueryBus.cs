using Truss.Results;

namespace Truss.Application.Cqrs.Queries;

/// <summary>
/// Bus for sending queries into the mediator system
/// </summary>
public interface IQueryBus
{
    /// <summary>
    /// Bus a query
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    /// <returns></returns>
    public Task<Result<TResult>> SendQuery<TQuery, TResult>(TQuery query)
        where TQuery : Query<TResult>;
}
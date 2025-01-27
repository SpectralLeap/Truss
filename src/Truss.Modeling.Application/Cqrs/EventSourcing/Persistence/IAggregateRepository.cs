using Truss.Modeling.Domain.Entities;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Persistence;

/// <summary>
/// Access a stream to write events
/// </summary>
public interface IAggregateRepository
{
    /// <summary>
    /// Store an aggregate to the event stream
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TId"></typeparam>
    /// <returns></returns>
    public Task<Result<Nil>> StoreAsync<TId>(
        IAggregateRoot<TId> aggregate,
        CancellationToken ct = default
    ) where TId : AggregateId<Guid>;

    /// <summary>
    /// Store an aggregate to the event stream
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TId"></typeparam>
    /// <returns></returns>
    public Task<Result<Nil>> StoreAsync(
        IAggregateRoot<Guid> aggregate,
        CancellationToken ct = default
    );

    /// <summary>
    /// Load an aggregate by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ct"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<Result<T>> LoadAsync<T>(
        Guid id,
        int? version = null,
        CancellationToken ct = default
    ) where T : class, IAggregateRoot<Guid>;

    /// <summary>
    /// Load an aggregate by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ct"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<Result<T>> LoadAsync<T>(
        AggregateId<Guid> id,
        int? version = null,
        CancellationToken ct = default
    ) where T : class, IAggregateRoot;
}

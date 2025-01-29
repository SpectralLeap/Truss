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
    /// <returns>
    /// A <see cref="Result{T}"/> indicating success or failure
    /// </returns>
    public Task<Result<Nil>> StoreAsync<TId>(
        IAggregate<TId> aggregate,
        CancellationToken ct = default
    ) where TId : AggregateId<Guid>;

    /// <summary>
    /// Store an aggregate to the event stream
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// A <see cref="Result{T}"/> indicating success or failure
    /// </returns>
    public Task<Result<Nil>> StoreAsync(
        IAggregate<Guid> aggregate,
        CancellationToken ct = default
    );

    /// <summary>
    /// Load an aggregate by its id
    /// </summary>
    /// <param name="id">
    /// The id of the aggregate to load
    /// </param>
    /// <param name="version">
    /// The version of the aggregate to load
    /// </param>
    /// <param name="ct">
    /// The cancellation token
    /// </param>
    /// <typeparam name="TAggregate">
    /// The type of the aggregate
    /// </typeparam>
    /// <returns>
    /// A <see cref="Result{T}"/> indicating success or failure with the loaded aggregate on success
    /// </returns>
    public Task<Result<TAggregate>> LoadAsync<TAggregate>(
        Guid id,
        int? version = null,
        CancellationToken ct = default
    ) where TAggregate : class, IAggregate<Guid>;

    /// <summary>
    /// Load an aggregate by its id
    /// </summary>
    /// <param name="id">
    /// The id of the aggregate to load
    /// </param>
    /// <param name="version">
    /// The version of the aggregate to start from
    /// </param>
    /// <param name="ct">
    /// The cancellation token
    /// </param>
    /// <typeparam name="TAggregate">
    /// The type of the aggregate
    /// </typeparam>
    /// <returns>
    /// A <see cref="Result{T}"/> indicating success or failure with the loaded aggregate on success
    /// </returns>
    public Task<Result<TAggregate>> LoadAsync<TAggregate>(
        AggregateId<Guid> id,
        int? version = null,
        CancellationToken ct = default
    ) where TAggregate : class, IAggregate;
}

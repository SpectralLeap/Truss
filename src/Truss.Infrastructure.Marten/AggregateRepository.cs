using Marten;
using Truss.Modeling.Application.Cqrs.EventSourcing.Persistence;
using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.Events;
using Truss.Monads.Results;

namespace Truss.Infrastructure.Marten;

internal sealed class AggregateRepository 
    : IAggregateRepository
{
    private readonly IDocumentStore _documentStore;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public AggregateRepository(
        IDocumentStore documentStore,
        IDomainEventDispatcher domainEventDispatcher
    )
    {
        _documentStore = documentStore;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Result<Nil>> StoreAsync<TId>(
        IAggregateRoot<TId> aggregate,
        CancellationToken ct = default
    ) where TId : AggregateId<Guid>
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        ArgumentNullException.ThrowIfNull(aggregate.Id);

        await using var session = await _documentStore
            .LightweightSerializableSessionAsync(token: ct);

        // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
        var events = aggregate.PendingEvents;

        session.Events.Append(
            aggregate.Id.Value,
            aggregate.Version,
            events
        );

        await session.SaveChangesAsync(ct);

        await _domainEventDispatcher.DispatchAndClearDomainEvents(
            aggregate,
            ct
        );

        return Result.Success();
    }

    public async Task<Result<Nil>> StoreAsync(
        IAggregateRoot<Guid> aggregate,
        CancellationToken ct = default
    )
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        ArgumentNullException.ThrowIfNull(aggregate.Id);

        await using var session = await _documentStore
            .LightweightSerializableSessionAsync(token: ct);

        // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
        var events = aggregate.PendingEvents;

        session.Events.Append(
            aggregate.Id,
            aggregate.Version,
            events
        );

        await session.SaveChangesAsync(ct);

        await _domainEventDispatcher.DispatchAndClearDomainEvents(
            aggregate,
            ct
        );

        return Result.Success();
    }

    public async Task<Result<T>> LoadAsync<T>(
        Guid id,
        int? version = null,
        CancellationToken ct = default
    ) where T : class, IAggregateRoot<Guid>
    {
        ArgumentNullException.ThrowIfNull(id);

        await using var session = await _documentStore
            .LightweightSerializableSessionAsync(token: ct);

        var aggregate = await session.Events.AggregateStreamAsync<T>(
            id,
            version ?? 0,
            token: ct
        );

        if (aggregate is null)
        {
            return Result.Fail($"No aggregate by id {id}.");
        }

        return Result.Success(aggregate);
    }

    public async Task<Result<T>> LoadAsync<T>(
        AggregateId<Guid> id,
        int? version = null,
        CancellationToken ct = default
    ) where T : class, IAggregateRoot
    {
        ArgumentNullException.ThrowIfNull(id);

        await using var session = await _documentStore
            .LightweightSerializableSessionAsync(token: ct);

        var aggregate = await session.Events.AggregateStreamAsync<T>(
            id.Value,
            version ?? 0,
            token: ct
        );

        if (aggregate is null)
        {
            return Result.Fail($"No aggregate by id {id}.");
        }

        return Result.Success(aggregate);
    }
}
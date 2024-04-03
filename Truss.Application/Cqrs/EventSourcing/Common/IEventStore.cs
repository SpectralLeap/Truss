using Truss.Application.Abstractions.Domain;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Common;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventStore
{
    public Task<Result<None>> Write(ChangeEventPayload @event);
    Task<Result<IAsyncEnumerable<ChangeEventPayload>>> Read(AggregateRootId<Guid> id);
}
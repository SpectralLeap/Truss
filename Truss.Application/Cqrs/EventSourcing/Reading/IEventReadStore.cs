using Truss.Application.Cqrs.EventSourcing.Events;
using Truss.Domain.Entities;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Reading;

public interface IEventReadStore
{
    Task<Result<IAsyncEnumerable<ChangeEventPayload>>> Read(AggregateRootId<Guid> id);
}
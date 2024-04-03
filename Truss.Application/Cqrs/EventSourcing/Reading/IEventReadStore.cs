using Truss.Application.Abstractions.Domain;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Common;

public interface IEventReadStore
{
    Task<Result<IAsyncEnumerable<ChangeEventPayload>>> Read(AggregateRootId<Guid> id);
}
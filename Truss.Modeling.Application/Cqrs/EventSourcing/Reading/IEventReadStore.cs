using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Reading;

public interface IEventReadStore
{
    Task<Result<IAsyncEnumerable<ChangeEventPayload>>> Read(AggregateRootId<Guid> id);
}
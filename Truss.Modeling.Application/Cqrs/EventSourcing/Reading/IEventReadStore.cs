using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Reading;

public interface IEventReadStore
{
    IAsyncEnumerable<ChangeEventPayload> Read(AggregateRootId<Guid> id);
}
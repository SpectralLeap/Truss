using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Domain.Entities;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventStore
{
    public Task<Result<Nil>> Write(
        ChangeEventPayload @event,
        CancellationToken cancellationToken
    );
    
    IAsyncEnumerable<ChangeEventPayload> Read(AggregateRootId<Guid> id);
}
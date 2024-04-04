using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventWriteStore
{
    public Task<Result<Nil>> Write(ChangeEventPayload @event);
}
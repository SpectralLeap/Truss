using Truss.Application.Cqrs.EventSourcing.Events;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventWriteStore
{
    public Task<Result<Nil>> Write(ChangeEventPayload @event);
}
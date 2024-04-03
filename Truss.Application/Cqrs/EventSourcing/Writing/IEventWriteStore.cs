using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Common;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventWriteStore
{
    public Task<Result<None>> Write(ChangeEventPayload @event);
}
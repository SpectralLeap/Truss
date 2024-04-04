using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Reading;

/// <summary>
/// Accesses an event stream for reading
/// </summary>
public interface IAggregateEventStreamReader
{
    /// <summary>
    /// Read the events from a stream
    /// </summary>
    /// <param name="id">The id of the aggregate stream to read. Corresponds to the aggregate's id</param>
    /// <returns></returns>
    public Task<Result<IAsyncEnumerable<ChangeEvent>>> ReadEventStream(AggregateRootId<Guid> id);
}
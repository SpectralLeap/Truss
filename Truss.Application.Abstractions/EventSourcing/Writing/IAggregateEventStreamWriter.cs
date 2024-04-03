using Truss.Application.Abstractions.Domain;
using Truss.Results;

namespace Truss.Application.Abstractions.EventSourcing.Writing;

/// <summary>
/// Access a stream to write events
/// </summary>
public interface IAggregateEventStreamWriter
{
    /// <summary>
    /// Append the events to a stream
    /// </summary>
    /// <returns></returns>
    Task<Result<None>> WriteToStream<TId>(IEventSourcedAggregateRoot<TId> aggregate) 
        where TId : AggregateRootId<Guid>;
}

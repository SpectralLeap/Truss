using Truss.Domain.Entities;
using Truss.Domain.EventSourcing;
using Truss.Results;

namespace Truss.Application.Cqrs.EventSourcing.Writing;

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

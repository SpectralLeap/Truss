using Truss.Modeling.Domain.Entities;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

/// <summary>
/// Access a stream to write events
/// </summary>
public interface IAggregateEventStreamWriter
{
    /// <summary>
    /// Append the events to a stream
    /// </summary>
    /// <returns></returns>
    Task<Result<Nil>> WriteToStream<TId>(IEventSourcedAggregateRoot<TId> aggregate) 
        where TId : AggregateRootId<Guid>;
}

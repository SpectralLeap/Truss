using System.Diagnostics.CodeAnalysis;
using Truss.Domain.Entities;

namespace Truss.Domain.EventSourcing;

/// <summary>
/// Represents an new aggregate entity has been created
/// </summary>
/// <typeparam name="TId"></typeparam>
[ExcludeFromCodeCoverage]
public abstract record CreationEvent<TId> : ChangeEvent 
    where TId : AggregateRootId<Guid>
{
    /// <summary>
    /// Represents a new aggregate entity that has been created.
    /// </summary>
    /// <typeparam name="TId">The type of the aggregate root ID.</typeparam>
    protected CreationEvent(Guid aggregateId)
        : base(aggregateId)
    {
    }
}
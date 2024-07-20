using System.Diagnostics.CodeAnalysis;
using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.EventSourcing;

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
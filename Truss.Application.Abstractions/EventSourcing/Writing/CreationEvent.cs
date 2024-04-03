using System.Diagnostics.CodeAnalysis;
using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an new aggregate entity has been created
/// </summary>
/// <typeparam name="T"></typeparam>
[ExcludeFromCodeCoverage]
public abstract record CreationEvent<T> : ChangeEvent where T : AggregateRootId<Guid>
{
    /// <summary>
    /// Represents an new aggregate entity has been created
    /// </summary>
    /// <param name="aggregateId">Aggregate Id</param>
    /// <typeparam name="T"></typeparam>
    protected CreationEvent(T aggregateId) : base(aggregateId.Value)
    {
    }
}
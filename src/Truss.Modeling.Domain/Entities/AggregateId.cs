namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// A typed Id for an Aggregate Root
/// </summary>
/// <typeparam name="TId">
/// The type of the Id
/// </typeparam>
public record AggregateId<TId> : EntityId<TId>
{
    /// <inheritdoc />
    protected AggregateId(TId Value) : base(Value)
    {
    }
}
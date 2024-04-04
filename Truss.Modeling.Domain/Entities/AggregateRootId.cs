namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// The Id for an Aggregate Root
/// </summary>
/// <typeparam name="TId"></typeparam>
public record AggregateRootId<TId> : EntityId<TId>
{
    /// <summary>
    /// The Id for an Aggregate Root
    /// </summary>
    /// <param name="Value"></param>
    /// <typeparam name="TId"></typeparam>
    protected AggregateRootId(TId Value) : base(Value)
    {
    }
    
}
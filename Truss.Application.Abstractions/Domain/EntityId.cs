namespace Truss.Application.Abstractions.Domain;

/// <summary>
/// Base object for ids used by entity types
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract record EntityId<TId>
{
    /// <summary>
    /// The Id Value
    /// </summary>
    public TId Value { get; }
    
    /// <summary>
    /// Base object for ids used by entity types
    /// </summary>
    /// <param name="Value"></param>
    /// <typeparam name="TId"></typeparam>
    protected EntityId(TId Value)
    {
        this.Value = Value;
    }
}


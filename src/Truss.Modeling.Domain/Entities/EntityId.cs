namespace Truss.Modeling.Domain.Entities;

/// <summary>
/// A typed Id for an Entity
/// </summary>
/// <typeparam name="TId">
/// The type of the Id
/// </typeparam>
public abstract record EntityId<TId>
{
    /// <summary>
    /// The Id Value
    /// </summary>
    public TId Value { get; private set; }
    
    /// <summary>
    /// Base object for ids used by entity types
    /// </summary>
    /// <param name="Value">
    /// The Id Value
    /// </param>
    protected EntityId(TId Value)
    {
        this.Value = Value;
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Value?.ToString();
    }
}


using Newtonsoft.Json;

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
    [JsonProperty]
    public TId Value { get; private set; }
    
    /// <summary>
    /// Base object for ids used by entity types
    /// </summary>
    /// <param name="Value"></param>
    /// <typeparam name="TId"></typeparam>
    protected EntityId(TId Value)
    {
        this.Value = Value;
    }

    public static implicit operator TId(EntityId<TId> id) => id.Value;
}


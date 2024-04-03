namespace Truss.Domain.Entities;

/// <summary>
/// Base object in DDD
/// An entity with a strongly typed identifier
/// </summary>
/// <typeparam name="TId"><see cref="Guid"/> is generally recommended</typeparam>
public abstract class Entity<TId>
{
    /// <summary>
    /// The unique Id of the entity
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">The strictly typed Id</param>
    /// <exception cref="ArgumentNullException">If the id is null</exception>
    /// <exception cref="ArgumentNullException">If the eventAggregator is null</exception>
    protected Entity(TId id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));
        
        Id = id;
    }

    /// <summary>
    /// Determines if entities are the same.
    /// If entities are the same type and have the same
    /// id they are the same entity
    /// </summary>
    /// <returns></returns>
    public sealed override bool Equals(object? obj)
    {
        return obj is Entity<TId> other && Id!.Equals(other.Id);
    }

    
    /// <inheritdoc />
    public sealed override int GetHashCode()
    {
        return Id!.GetHashCode();
    }

}
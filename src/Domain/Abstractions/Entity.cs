namespace Domain.Abstractions;

/// <summary>
/// Base class for entities
/// </summary>
/// <typeparam name="TId">Type of the entity identifier</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : ValueObject
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    public TId Id { get; protected set; } = null!;

    /// <summary>
    /// Checks if two entities are equal by their identifiers
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool Equals(Entity<TId>? other) => other is not null && Id.Equals(other.Id);
    /// <summary>
    /// Checks if two entities are equal by their identifiers
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is Entity<TId> other && Equals(other);
    /// <summary>
    /// Returns entity identifier hash code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Id.GetHashCode();
    /// <summary>
    /// Checks if two entities are equal by their identifiers
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(Entity<TId>? a, Entity<TId>? b) => a is not null && a.Equals(b);
    /// <summary>
    /// Checks if two entities are not equal by their identifiers
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);
}

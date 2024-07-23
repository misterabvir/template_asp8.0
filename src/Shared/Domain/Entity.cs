namespace Shared.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : ValueObject
{
    public TId Id { get; protected set; } = null!;
    public bool Equals(Entity<TId>? other) => other is not null && Id.Equals(other.Id);
    public override bool Equals(object? obj) => obj is Entity<TId> other && Id.Equals(other.Id);
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Entity<TId>? a, Entity<TId>? b) => a is not null && a.Equals(b);
    public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !(a == b);
}

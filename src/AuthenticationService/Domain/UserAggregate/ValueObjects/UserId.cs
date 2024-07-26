using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents a unique identifier for a user.
/// </summary>
/// <param name="Value">The unique identifier value.</param>
public sealed record UserId : ValueObject
{
    public Guid Value { get; init; }
    private UserId(Guid value) => Value = value;
    public static UserId CreateUnique() => new (Guid.NewGuid());
    public static UserId Create(Guid value) => new (value);
    public static implicit operator Guid(UserId userId) => userId.Value;    
}

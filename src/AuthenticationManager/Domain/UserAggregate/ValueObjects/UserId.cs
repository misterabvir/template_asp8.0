using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record UserId : ValueObject
{
    public Guid Value { get; init; }
    private UserId(Guid value) => Value = value;
    public static UserId CreateUnique() => new (Guid.NewGuid());
    public static UserId Create(Guid value) => new (value);
}

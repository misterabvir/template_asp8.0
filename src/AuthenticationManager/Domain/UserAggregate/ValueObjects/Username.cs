using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record Username : ValueObject
{
    public string Value { get; init; }
    private Username(string value) => Value = value;
    public static Username Create(string value) => new (value);
}

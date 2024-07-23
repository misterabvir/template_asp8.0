using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record Bio : ValueObject
{
    public string Value { get; init; }
    private Bio(string value) => Value = value;
    public static Bio Create(string value) => new (value);
    public static Bio Empty => new (string.Empty);
}

using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public record LastName : ValueObject
{
    public string Value { get; init; }
    private LastName(string value) => Value = value;
    public static LastName Create(string value) => new (value);
    public static LastName Empty => new (string.Empty);
}

using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed  record Website : ValueObject
{
    public string Value { get; init; }
    private Website(string value) => Value = value;
    public static Website Create(string value) => new (value);
    public static Website Empty => new (string.Empty);
}

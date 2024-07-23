using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed  record Email : ValueObject
{
    public string Value { get; init; }
    private Email(string value) => Value = value;
    public static Email Create(string value) => new (value);
}

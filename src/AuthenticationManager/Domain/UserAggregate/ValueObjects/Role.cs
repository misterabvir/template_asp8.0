using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public record Role : ValueObject
{
    public string Value { get; init; }
    private Role(string value) => Value = value;
    public static Role Create(string value) => new (value);
    public static Role Administrator => new ("Administrator");
    public static Role User => new ("User");
}

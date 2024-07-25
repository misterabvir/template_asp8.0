using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record Username : ValueObject
{
    public string Value { get; init; }
    private Username(string value) => Value = value;
    public static Username Create(string value) => new (value);
    public static implicit operator string(Username username) => username.Value;
    public const string Regex = @"^[A-Za-z0-9_-]{6,50}$ ";
}

using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Last name of a user
/// </summary>
/// <param name="Value"></param>
public record LastName : ValueObject
{
    public string Value { get; init; }
    private LastName(string value) => Value = value;
    public static LastName Create(string value) => new (value);
    public static LastName Empty => new (string.Empty);
}

using Domain.Abstractions;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Biography of user
/// </summary>
/// <param name="Value"></param>
/// </summary>
public sealed record Bio : ValueObject
{
    public string Value { get; init; }
    private Bio(string value) => Value = value;
    public static Bio Create(string value) => new (value);
    public static Bio Empty => new (string.Empty);
}

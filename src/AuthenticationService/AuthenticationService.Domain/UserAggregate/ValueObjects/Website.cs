using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents the website value object.
/// </summary>
/// <param name="Value">The website value.</param>
/// <param name="Empty">The empty website value.</param>
/// <remarks>
/// This value object is used to represent the website of a user.
/// </remarks>

public sealed  record Website : ValueObject
{
    public string Value { get; init; }
    private Website(string value) => Value = value;
    public static Website Create(string value) => new (value);
    public static Website Empty => new (string.Empty);
}

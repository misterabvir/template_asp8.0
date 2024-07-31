using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents the username value object.
/// </summary>
/// <param name="Value">The username value.</param>
/// <remarks>
/// The username value object is a value object that represents the username of a user.
/// It is used to ensure that the username is valid and follows the required format.
/// The username value object is immutable and can only be created using the <see cref="Create"/> method.
/// The username value object is also used as the username of a user in the AuthenticationService.Domain model.
/// </remarks>

public sealed record Username : ValueObject
{
    public string Value { get; init; }
    private Username(string value) => Value = value;
    public static Username Create(string value) => new (value);
    public static implicit operator string(Username username) => username.Value;
    public const string Regex = @"^[A-Za-z0-9_-]{6,50}$ ";
}

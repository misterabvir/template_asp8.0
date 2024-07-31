using Domain.Abstractions;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Url to user's profile picture
/// </summary>
public record ProfilePicture : ValueObject
{
    public string Value { get; init; }
    private ProfilePicture(string value) => Value = value;
    public static ProfilePicture Create(string value) => new (value);
    public static ProfilePicture Empty => new (string.Empty);
}

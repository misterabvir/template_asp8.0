using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Url to user's cover picture
/// </summary>
/// </summary>
public record CoverPicture : ValueObject
{
    public string Value { get; init; }
    private CoverPicture(string value) => Value = value;
    public static CoverPicture Create(string value) => new (value);
    public static CoverPicture Empty => new (string.Empty);
}   

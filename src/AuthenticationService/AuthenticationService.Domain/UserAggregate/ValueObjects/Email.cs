using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Email contact
/// </summary>
public sealed record Email : ValueObject
{
    public string Value { get; init; }
    private Email(string value) => Value = value;
    public static Email Create(string value) => new (value);
    
    public static implicit operator string(Email email) => email.Value;
    public const string Regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
}

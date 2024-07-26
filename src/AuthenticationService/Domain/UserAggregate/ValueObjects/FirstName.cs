using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// First Name 
/// </summary>
public record FirstName : ValueObject
{
    public string Value { get; init; }
    private FirstName(string value) => Value = value;
    public static FirstName Create(string value) => new (value);
    public static FirstName Empty => new (string.Empty);
}

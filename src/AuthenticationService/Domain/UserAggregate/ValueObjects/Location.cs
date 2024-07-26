using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Location of the user
/// </summary>
public sealed  record Location : ValueObject
{
    public string Value { get; init; }
    private Location(string value) => Value = value;
    public static Location Create(string value) => new (value);
    public static Location Empty => new (string.Empty);
    public const string None = "None";
}

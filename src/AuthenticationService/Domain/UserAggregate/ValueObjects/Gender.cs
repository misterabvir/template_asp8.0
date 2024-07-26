using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Gender
/// </summary>
public sealed  record Gender : ValueObject
{
    public string Value { get; init; }
    private Gender(string value) => Value = value;
    public static Gender Create(string value) => new (value);
    public static Gender Male => new ("Male");
    public static Gender Female => new ("Female");
    public static Gender Other => new ("Other");
    public static Gender Empty => new (string.Empty);
    public const string None = "None";
}

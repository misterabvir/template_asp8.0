using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

/// <summary>
/// Date of Birth
/// </summary>
public sealed  record Birthday : ValueObject
{
    public DateOnly Value { get; init; }
    private Birthday(DateOnly value) => Value = value;
    public static Birthday Create(DateOnly value) => new (value);

    public bool IsAdult() => Age() >= 18;

    public int Age()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - Value.Year;
        if (Value.AddYears(age) > today)
            age--;
        return age;
    }
    public static Birthday Empty => new (DateOnly.FromDateTime(DateTime.MinValue));
}
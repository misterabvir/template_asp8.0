using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public record CoverPicture : ValueObject
{
    public string Value { get; init; }
    private CoverPicture(string value) => Value = value;
    public static CoverPicture Create(string value) => new (value);
    public static CoverPicture Empty => new (string.Empty);
}   

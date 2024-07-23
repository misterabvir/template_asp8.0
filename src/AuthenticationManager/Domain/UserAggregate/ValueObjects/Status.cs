using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record Status : ValueObject
{
    public string Value { get; init; }
    private Status(string value) => Value = value;
    public static Status Create(string value) => new (value);
    public static Status NotVerified => new ("NotVerified");
    public static Status Active => new ("Active");
    public static Status Suspended => new ("Suspended");
}

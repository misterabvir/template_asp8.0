using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed record Salt : ValueObject
{
    public byte[] Value { get; init; }
    private Salt(byte[] value) => Value = value;
    public static Salt Create(byte[] value) => new (value);
}

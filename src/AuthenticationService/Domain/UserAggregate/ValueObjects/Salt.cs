using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents a salt value used for password hashing.
/// </summary>
/// <param name="Value">The byte array representing the salt.</param>
public sealed record Salt : ValueObject
{
    public byte[] Value { get; init; }
    private Salt(byte[] value) => Value = value;
    public static Salt Create(byte[] value) => new (value);
}

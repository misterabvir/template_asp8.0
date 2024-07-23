using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

public sealed  record Password : ValueObject
{
    public byte[] Value { get; init; }
    private Password(byte[] value) => Value = value;
    public static Password Create(byte[] value) => new (value);
    public bool IsSameAs(Password password) => Value.SequenceEqual(password.Value);
}

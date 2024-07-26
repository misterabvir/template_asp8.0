using Shared.Domain;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Password to access on the system
/// </summary>
public sealed  record Password : ValueObject
{
    public byte[] Value { get; init; }
    private Password(byte[] value) => Value = value;
    public static Password Create(byte[] value) => new (value);
    public bool IsSameAs(byte[] password) => Value.SequenceEqual(password);

    public const string Regex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*_-]).{8,}$";
}

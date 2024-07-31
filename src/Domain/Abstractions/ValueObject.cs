namespace Domain.Abstractions;

/// <summary>
/// Value Object
/// </summary>
/// <remarks>
/// Value objects are objects that are meant to be immutable and have no identity.
/// Two value objects are considered equal if their state is the same.
/// </remarks>

public abstract record ValueObject;

using Shared.Domain;

namespace Domain.UserAggregate.Events;

public record UserCreatedDomainEvent(Guid UserId, string Email) : IDomainEvent;
public record UserVerifiedDomainEvent(Guid UserId) : IDomainEvent;

public record UserSuspendedDomainEvent(Guid UserId) : IDomainEvent;
public record UserPasswordUpdatedDomainEvent(Guid UserId) : IDomainEvent;
public record UserDataUpdatedDomainEvent(Guid UserId, string Email, string Username) : IDomainEvent;
public record UserProfileUpdatedDomainEvent(Guid UserId) : IDomainEvent;
public record UserRoleChangedDomainEvent(Guid UserId, string Role) : IDomainEvent;

public static class RegisteredDomainEvents
{
    public static readonly Dictionary<string, Type> Events = new()
    {
        { nameof(UserCreatedDomainEvent), typeof(UserCreatedDomainEvent)},
        { nameof(UserVerifiedDomainEvent), typeof(UserVerifiedDomainEvent)},
        { nameof(UserSuspendedDomainEvent), typeof(UserSuspendedDomainEvent)},
        { nameof(UserPasswordUpdatedDomainEvent), typeof(UserPasswordUpdatedDomainEvent)},
        { nameof(UserDataUpdatedDomainEvent), typeof(UserDataUpdatedDomainEvent)},
        { nameof(UserProfileUpdatedDomainEvent), typeof(UserProfileUpdatedDomainEvent)},
        { nameof(UserRoleChangedDomainEvent), typeof(UserRoleChangedDomainEvent)}
    };
}
using Shared.Domain;

namespace Domain.UserAggregate.Events;


/// <summary>
/// User Created Domain Event
/// This event is triggered when a new user is created in the system
/// </summary>
public record UserCreatedDomainEvent(Guid UserId, string Username, string Email) : IDomainEvent;

/// <summary>
/// User Verified Domain Event
/// This event is triggered when a new user is verify email after register
/// </summary>
public record UserVerifiedDomainEvent(Guid UserId, string Username, string Email) : IDomainEvent;

/// <summary>
/// User Suspended Domain Event
/// This event is triggered when user suspend self account
/// </summary>
public record UserSuspendedDomainEvent(Guid UserId, string Email, string Username) : IDomainEvent;

/// <summary>
/// User Password Updated Domain Event
/// This event is triggered when user change password
/// </summary>
public record UserPasswordChangedDomainEvent(Guid UserId, string Email, string Username) : IDomainEvent;

/// <summary>
/// User Data Update Domain Event
/// This event is triggered when user change email or username
/// </summary>
public record UserUsernameUpdatedDomainEvent(Guid UserId, string Email, string Username, string Avatar) : IDomainEvent;

/// <summary>
/// User Profile Updated Domain Event
/// This event is triggered when user update self profile
/// </summary>
/// <param name="UserId"></param>
public record UserProfileUpdatedDomainEvent(Guid UserId, string Email, string Username, string Avatar) : IDomainEvent;

/// <summary>
/// User Role Changed Domain Event
/// This event is triggered when user role changed by admin
/// </summary>
public record UserRoleChangedDomainEvent(Guid UserId, string Email, string Username, string Role) : IDomainEvent;

/// <summary>
/// Registered Domain Events
/// This class is used to register all domain events
/// </summary>
/// <remarks>
/// This class is used to register all domain events in the system
/// </remarks>
public static class RegisteredDomainEvents
{
    public static readonly Dictionary<string, Type> Events = new()
    {
        { nameof(UserCreatedDomainEvent), typeof(UserCreatedDomainEvent)},
        { nameof(UserVerifiedDomainEvent), typeof(UserVerifiedDomainEvent)},
        { nameof(UserSuspendedDomainEvent), typeof(UserSuspendedDomainEvent)},
        { nameof(UserPasswordChangedDomainEvent), typeof(UserPasswordChangedDomainEvent)},
        { nameof(UserUsernameUpdatedDomainEvent), typeof(UserUsernameUpdatedDomainEvent)},
        { nameof(UserProfileUpdatedDomainEvent), typeof(UserProfileUpdatedDomainEvent)},
        { nameof(UserRoleChangedDomainEvent), typeof(UserRoleChangedDomainEvent)}
    };
}

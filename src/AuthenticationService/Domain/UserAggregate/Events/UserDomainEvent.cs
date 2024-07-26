using Shared.Domain;

namespace Domain.UserAggregate.Events;


/// <summary>
/// User Created Domain Event
/// This event is triggered when a new user is created in the system
/// </summary>
/// <param name="UserId"></param>
/// <param name="Username"></param>
/// <param name="Email"></param>
public record UserCreatedDomainEvent(Guid UserId, string Username, string Email) : IDomainEvent;

/// <summary>
/// User Verified Domain Event
/// This event is triggered when a new user is verify email after register
/// </summary>
/// <param name="UserId"></param>
public record UserVerifiedDomainEvent(Guid UserId) : IDomainEvent;

/// <summary>
/// User Suspended Domain Event
/// This event is triggered when user suspend self account
/// </summary>
/// <param name="UserId"></param>
public record UserSuspendedDomainEvent(Guid UserId) : IDomainEvent;

/// <summary>
/// User Password Updated Domain Event
/// This event is triggered when user change password
/// </summary>
/// <param name="UserId"></param>
public record UserPasswordUpdatedDomainEvent(Guid UserId) : IDomainEvent;

/// <summary>
/// User Data Update Domain Event
/// This event is triggered when user change email or username
/// </summary>
/// <param name="UserId"></param>
/// <param name="Email"></param>
/// <param name="Username"></param>
public record UserDataUpdatedDomainEvent(Guid UserId, string Email, string Username) : IDomainEvent;

/// <summary>
/// User Profile Updated Domain Event
/// This event is triggered when user update self profile
/// </summary>
/// <param name="UserId"></param>
public record UserProfileUpdatedDomainEvent(Guid UserId) : IDomainEvent;

/// <summary>
/// User Role Changed Domain Event
/// This event is triggered when user role changed by admin
/// </summary>
/// <param name="UserId"></param>
/// <param name="Role"></param>
public record UserRoleChangedDomainEvent(Guid UserId, string Role) : IDomainEvent;

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
        { nameof(UserPasswordUpdatedDomainEvent), typeof(UserPasswordUpdatedDomainEvent)},
        { nameof(UserDataUpdatedDomainEvent), typeof(UserDataUpdatedDomainEvent)},
        { nameof(UserProfileUpdatedDomainEvent), typeof(UserProfileUpdatedDomainEvent)},
        { nameof(UserRoleChangedDomainEvent), typeof(UserRoleChangedDomainEvent)}
    };
}

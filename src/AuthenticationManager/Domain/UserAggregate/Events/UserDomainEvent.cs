using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate.Events;

public record UserCreatedDomainEvent(User User) : IDomainEvent;
public record UserVerifiedDomainEvent(User User) : IDomainEvent;
public record UserSuspendedDomainEvent(User User) : IDomainEvent;
public record UserPasswordUpdatedDomainEvent(User User) : IDomainEvent;
public record UserEmailUpdatedDomainEvent(User User) : IDomainEvent;
public record UsernameUpdatedDomainEvent(User User) : IDomainEvent;
public record UserRoleAddedDomainEvent(User User) : IDomainEvent;
public record UserProfileUpdatedDomainEvent(User User) : IDomainEvent;
namespace Shared.Events;


public record UserVerifiedEvent(Guid UserId, string Username, string Email, string VerificationCode);
public record UserPasswordChangedEvent(Guid UserId, string Username, string Email);
public record UserSuccessVerifiedEvent(Guid UserId, string Username, string Email);
public record UserConfirmedEvent(Guid UserId, string Username, string Email);
public record UserSuspendedEvent(Guid UserId, string Username, string Email);
public record UserAccountDataChangedEvent(Guid UserId, string Username, string Avatar);
public record UserRoleChangedEvent(Guid UserId, string Username, string Email, string Role);
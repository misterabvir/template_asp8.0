namespace Shared.Events;


public record UserVerificationEvent(string Username, string Email, string VerificationCode);
public record UserWarningActivityEvent(string Username, string Email);
public record UserConfirmedEvent(string Username, string Email);
public record UserAccountDataChangedEvent(Guid UserId, string Username, string Avatar);
public record UserRoleChangedEvent(Guid UserId, string Username, string Email, string Role);
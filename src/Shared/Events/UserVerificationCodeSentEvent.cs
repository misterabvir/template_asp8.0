namespace Shared.Events;

public record UserVerificationCodeSentEvent(string Email, string VerificationCode);
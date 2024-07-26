namespace Shared.Events;

/// <summary>
/// Event that is triggered when a user verification code is sent to the user's email
/// </summary>
/// </summary>
/// <param name="Email"></param>
/// <param name="VerificationCode"></param>
public record UserVerificationCodeSentEvent(string Username, string Email, string VerificationCode);

public record UserWelcomeEmailSentEvent(string Username, string Email);
namespace MailService.Application.Common.Services;

public interface IEmailSender
{
    Task SendVerificationEmailAsync(string email, string username, string code);
    Task SendWelcomeEmailAsync(string email, string username);
    Task SendWarningEmailAsync(string email, string username);
    Task SendRoleEmailAsync(string email, string username, string role);
}

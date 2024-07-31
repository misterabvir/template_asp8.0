using MailService.Application.Common.Services;

using MailService.Domain;

using MailService.Infrastructure.Repositories;

using MailKit.Net.Smtp;
using MimeKit;

namespace MailService.Infrastructure;


public static partial class EmailSender
{

    public class Service(Settings settings, TemplateRepository templateRepository) : IEmailSender
    {
        public async Task SendVerificationEmailAsync(string email, string username, string code)
        {
            var template = await templateRepository.GetTemplateByName(Constants.TemplateNames.VerificationEmail);
            template.Body = template.Body.Replace("{username}", username).Replace("{verificationCode}", code);
            await SendEmail(email, template.Header, template.Body);
        }

        public async Task SendWelcomeEmailAsync(string email, string username)
        {
            var template = await templateRepository.GetTemplateByName(Constants.TemplateNames.WelcomeEmail);
            template.Body = template.Body.Replace("{username}", username);
            await SendEmail(email, template.Header, template.Body);
        }

        public async Task SendWarningEmailAsync(string email, string username)
        {
            var template = await templateRepository.GetTemplateByName(Constants.TemplateNames.WarningEmail);
            template.Body = template.Body.Replace("{username}", username);
            await SendEmail(email, template.Header, template.Body);
        }

        public async Task SendRoleEmailAsync(string email, string username, string role)
        {
            var template = await templateRepository.GetTemplateByName(Constants.TemplateNames.RoleEmail);
            template.Body = template.Body.Replace("{username}", username).Replace("{role}", role);
            await SendEmail(email, template.Header, template.Body);
        }

        private async Task SendEmail(string email, string subject, string body)
        {
            var to = new MailboxAddress("", email);
            var message = new MimeMessage();
            message.From.Add(settings.From);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(settings.Host, settings.Port, false);
            // await client.AuthenticateAsync(settings.Username, settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    } 
}
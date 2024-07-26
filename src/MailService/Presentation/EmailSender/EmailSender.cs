using MailKit.Net.Smtp;

using MimeKit;

namespace Presentation;

public interface IEmailSender
{
    Task SendVerificationEmailAsync(string email, string username, string code);
    Task SendWelcomeEmailAsync(string email, string username);
    Task SendWarningEmailAsync(string email, string username);
    Task SendRoleEmailAsync(string email, string username, string role);
}

public static class EmailSender
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
            throw new EmailSettingsNotConfiguredException();
        services.AddSingleton(settings);
        services.AddScoped<IEmailSender, Service>();
        return services;
    }

    public class EmailSettingsNotConfiguredException : Exception
    {
        public EmailSettingsNotConfiguredException() : base("Email settings not configured")
        {
        }
    }

    public class Settings
    {
        public const string SectionName = "Settings:Emails";
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Address { get; set; }
        public required string Password { get; set; }
        public MailboxAddress From  => new(Username, Address);
    }

    public class Service(Settings settings) : IEmailSender
    {
        public async Task SendVerificationEmailAsync(string email, string username, string code)
        {
            await SendEmail(email, Templates.VerificationEmailSubject, Templates.GetVerificationBodyEmail(username, code));
        }

        public async Task SendWelcomeEmailAsync(string email, string username)
        {
            await SendEmail(email, Templates.WelcomeEmailSubject, Templates.GetWelcomeBodyEmail(username));
        }

        public async Task SendWarningEmailAsync(string email, string username)
        {
            await SendEmail(email, Templates.WarningEmailSubject, Templates.GetWarningBodyEmail(username));
        }

        public async Task SendRoleEmailAsync(string email, string username, string role)
        {
            await SendEmail(email, Templates.RoleEmailTemplate, Templates.GetRoleBodyEmail(username, role));
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


    public static class Templates
    {
        public const string VerificationEmailSubject = "Email Verification";
        public const string WelcomeEmailSubject = "Welcome to Our App";
        public const string WarningEmailSubject = "Warning"; 
        
        public const string RoleEmailTemplate = "Your role in system has been changed";

        public static string GetRoleBodyEmail(string username, string role)
        {
            var body = File.ReadAllText($"{Environment.CurrentDirectory}/Templates/role_email.html");
            body = body.Replace("{{role}}", role);
            body = body.Replace("{{username}}", username);
            return body;
        }

        public static string GetVerificationBodyEmail(string username, string verificationCode)
        {
            var body = File.ReadAllText($"{Environment.CurrentDirectory}/Templates/verification_email.html");
            body = body.Replace("{{verificationCode}}", verificationCode);
            body = body.Replace("{{username}}", username);
            return body;
        }
        public static string GetWelcomeBodyEmail(string username)
        {
            var body = File.ReadAllText($"{Environment.CurrentDirectory}/Templates/welcome_email.html");
            body = body.Replace("{{username}}", username);
            return body;
        }

        public static string GetWarningBodyEmail(string username)
        {
            var body = File.ReadAllText($"{Environment.CurrentDirectory}/Templates/warning_email.html");
            body = body.Replace("{{username}}", username);
            return body;
        }
    }
}
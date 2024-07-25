using MailKit.Net.Smtp;

using MimeKit;


namespace EmailManager;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string content, EmailSender.EmailTarget emailTarget);
}

public static class EmailSender
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Settings:Emails").Get<Settings>() ??
            throw new Exception("Email settings not configured");
        services.AddSingleton(settings);
        services.AddScoped<IEmailSender, Service>();
        return services;
    }



    public class Settings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Address { get; set; }
        public required string Password { get; set; }
    }

    public class Service(Settings settings) : IEmailSender
    {
        public async Task SendEmailAsync(string to, string content, EmailTarget emailTarget)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings.Username, settings.Address));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = emailTarget switch
            {
                EmailTarget.Verification => "Verification Code",
                EmailTarget.Welcome => "Welcome",
                _ => "Notification"
            };

            message.Body = emailTarget switch
            {
                EmailTarget.Verification => new TextPart("html") { Text = Templates.GetVerificationBodyEmail(to, content) },
                EmailTarget.Welcome => new TextPart("html") { Text = Templates.GetWelcomeBodyEmail(to) },
                _ => new TextPart("html") { Text = "Notification" }
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(settings.Host, settings.Port, false);
                // await client.AuthenticateAsync(settings.Username, settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

    public enum EmailTarget
    {
        Verification,
        Welcome
    }


    public static class Templates
    {
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
    }
}
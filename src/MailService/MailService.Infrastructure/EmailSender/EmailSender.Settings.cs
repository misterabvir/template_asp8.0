using MailKit.Net.Smtp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MimeKit;

namespace MailService.Infrastructure;

public static partial class EmailSender
{

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

}
using MailService.Application.Common.Repositories;
using MailService.Application.Common.Services;

using MailService.Domain;

using MediatR;

namespace MailService.Application.Notifications;

public static class Welcome
{
    public record Notification(Guid UserId, string Email, string Username,  string Reason) : INotification;
    public class Handler(IUserRepository userRepository, IMessageRepository messageRepository, IEmailSender emailSender) : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetOrCreateAsync(notification.UserId, notification.Email, notification.Username, cancellationToken);
            await emailSender.SendWelcomeEmailAsync(user.Email, user.Username);
            await messageRepository.AddMessageAsync(user.UserId, Constants.Types.Welcome, notification.Reason, cancellationToken);
        }
    }
}
using MassTransit;
using MediatR;
using Shared.Events;
namespace Application.Users.Events;


public static class VerificationCodeSent
{
    public record Notification(string Username, string Email, string VerificationCode) : INotification;
    public class Handler(IPublishEndpoint endpoint) : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            await endpoint.Publish(new UserVerificationCodeSentEvent(notification.Username, notification.Email, notification.VerificationCode), cancellationToken);
        }
    }
}

using MassTransit;
using MediatR;
using Application.Events;
namespace AuthenticationService.Application.Users.Events;


public static class VerificationCodeSent
{
    public record Notification(Guid UserId, string Username, string Email, string VerificationCode) : INotification;
    public class Handler(IPublishEndpoint endpoint) : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            await endpoint.Publish(new UserVerifiedEvent(notification.UserId, notification.Username, notification.Email, notification.VerificationCode), cancellationToken);
        }
    }
}


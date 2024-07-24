using Domain.UserAggregate;
using MassTransit;
using MediatR;
using Shared.Events;
namespace Application.Users.Events;


public static class VerificationCodeSent
{
    public record Notification(User User, string VerificationCode) : INotification;
    public class Handler(IPublishEndpoint endpoint) : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            await endpoint.Publish(new UserVerificationCodeSentEvent(notification.User.Data.Email, notification.VerificationCode), cancellationToken);
        }
    }
}

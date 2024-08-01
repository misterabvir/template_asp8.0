using MassTransit;
using MediatR;
using Application.Events;

namespace MailService.Infrastructure.Consumers;

public class UserVerificationConsumer(
    IPublisher publisher) : 
    IConsumer<UserVerifiedEvent>
{
    public const string Reason = "Account must be verified";
    public async Task Consume(ConsumeContext<UserVerifiedEvent> context)
    {
            var notification = new Application.Notifications.Verification.Notification(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            Reason,
            context.Message.VerificationCode);
        await publisher.Publish(notification);   
    }   
}

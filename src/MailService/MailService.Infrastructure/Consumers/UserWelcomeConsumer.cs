using MassTransit;
using MediatR;
using Application.Events;
namespace MailService.Infrastructure.Consumers;

public class UserWelcomeConsumer(
    IPublisher publisher) :
    IConsumer<UserConfirmedEvent>
{
    public const string Reason = "User confirmed account";
    public async Task Consume(ConsumeContext<UserConfirmedEvent> context)
    {
        var notification = new Application.Notifications.Welcome.Notification(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            Reason);
        await publisher.Publish(notification);   
    }
}

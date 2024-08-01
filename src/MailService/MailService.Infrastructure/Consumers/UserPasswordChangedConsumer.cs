using MassTransit;
using MediatR;
using Application.Events;

namespace MailService.Infrastructure.Consumers;

public class UserPasswordChangedConsumer(
    IPublisher publisher) :
    IConsumer<UserPasswordChangedEvent>
{
    private const string Reason = "User password changed";
    
    public async Task Consume(ConsumeContext<UserPasswordChangedEvent> context)
    {
        var notification = new Application.Notifications.Warning.Notification(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            Reason);
        await publisher.Publish(notification);
    }
}


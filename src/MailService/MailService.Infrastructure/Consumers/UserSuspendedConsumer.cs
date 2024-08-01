using MassTransit;
using MediatR;
using Application.Events;

namespace MailService.Infrastructure.Consumers;

public class UserSuspendedConsumer(
    IPublisher publisher) :
    IConsumer<UserSuspendedEvent>
{
    private const string Reason = "Account suspended";


    public async Task Consume(ConsumeContext<UserSuspendedEvent> context)
    {
        var notification = new Application.Notifications.Warning.Notification(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            Reason);
        await publisher.Publish(notification);     
    }
}


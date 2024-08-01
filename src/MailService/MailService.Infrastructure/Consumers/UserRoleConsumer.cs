using MassTransit;
using MediatR;
using Application.Events;

namespace MailService.Infrastructure.Consumers;

public class UserRoleConsumer(
    IPublisher publisher) :
    IConsumer<UserRoleChangedEvent>
{
    public const string Reason = "User role changed to {0}";    
    public async Task Consume(ConsumeContext<UserRoleChangedEvent> context)
    {
        var notification = new MailService.Application.Notifications.RoleChanged.Notification(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            Reason: string.Format(Reason, context.Message.Role),
            context.Message.Role);
        await publisher.Publish(notification);
    }
}

using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserRoleConsumer(
    ILogger<UserRoleConsumer> logger,
    IEmailSender emailSender) :
    IConsumer<UserRoleChangedEvent>
{
    public Task Consume(ConsumeContext<UserRoleChangedEvent> context)
    {
        logger.LogInformation("User role changed email sent: {Email}", context.Message.Email);
        return emailSender.SendRoleEmailAsync(
            context.Message.Email,
            context.Message.Username,
            context.Message.Role);
    }
}

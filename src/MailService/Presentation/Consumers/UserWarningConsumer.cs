using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserWarningConsumer(
    ILogger<UserWarningConsumer> logger,
    IEmailSender emailSender) :
    IConsumer<UserWarningActivityEvent>
{
    public Task Consume(ConsumeContext<UserWarningActivityEvent> context)
    {
        logger.LogInformation("User warning email sent: {Email}", context.Message.Email);
        return emailSender.SendWarningEmailAsync(
            context.Message.Email,
            context.Message.Username);
    }
}




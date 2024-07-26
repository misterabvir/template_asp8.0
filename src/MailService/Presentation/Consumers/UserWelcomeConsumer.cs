using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserWelcomeConsumer(
    ILogger<UserWelcomeConsumer> logger,
    IEmailSender emailSender) :
    IConsumer<UserConfirmedEvent>
{
    public Task Consume(ConsumeContext<UserConfirmedEvent> context)
    {
        logger.LogInformation("User welcome email sent: {Email}", context.Message.Email);
        return emailSender.SendWelcomeEmailAsync(
            context.Message.Email,
            context.Message.Username);
    }
}

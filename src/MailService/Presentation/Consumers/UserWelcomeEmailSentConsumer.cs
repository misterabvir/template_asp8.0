using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserWelcomeEmailSentConsumer(
    ILogger<UserWelcomeEmailSentConsumer> logger,
    IEmailSender emailSender) :
    IConsumer<UserWelcomeEmailSentEvent>
{
    public Task Consume(ConsumeContext<UserWelcomeEmailSentEvent> context)
    {
        logger.LogInformation("User welcome email sent: {Email}", context.Message.Email);
        return emailSender.SendWelcomeEmailAsync(
            context.Message.Email,
            context.Message.Username);
    }
}
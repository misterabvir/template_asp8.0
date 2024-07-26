using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserVerificationConsumer(
    ILogger<UserVerificationConsumer> logger, 
    IEmailSender emailSender) : 
    IConsumer<UserVerificationEvent>
{
    public async Task Consume(ConsumeContext<UserVerificationEvent> context)
    {
        logger.LogInformation("User verification code sent: {Email}", context.Message.Email);
        await emailSender.SendVerificationEmailAsync(
            context.Message.Email,
            context.Message.Username,
            context.Message.VerificationCode);
    }   
}

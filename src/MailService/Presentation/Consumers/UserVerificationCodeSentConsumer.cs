using MassTransit;

using Shared.Events;

namespace Presentation.Consumers;

public class UserVerificationCodeSentConsumer(
    ILogger<UserVerificationCodeSentConsumer> logger, 
    IEmailSender emailSender) : 
    IConsumer<UserVerificationCodeSentEvent>
{
    public async Task Consume(ConsumeContext<UserVerificationCodeSentEvent> context)
    {
        logger.LogInformation("User verification code sent: {Email}", context.Message.Email);
        await emailSender.SendVerificationEmailAsync(
            context.Message.Email,
            context.Message.Username,
            context.Message.VerificationCode);
    }   
}

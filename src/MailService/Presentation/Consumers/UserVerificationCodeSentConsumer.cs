
using MassTransit;

using Shared.Events;

namespace EmailManager.Consumers;

public class UserVerificationCodeSentConsumer(ILogger<UserVerificationCodeSentConsumer> logger, IEmailSender emailSender) : IConsumer<UserVerificationCodeSentEvent>
{
    public async Task Consume(ConsumeContext<UserVerificationCodeSentEvent> context)
    {
        await Task.CompletedTask;
        logger.LogInformation("User verification code sent: {Email}", context.Message.Email);
        await emailSender.SendEmailAsync(
            context.Message.Email, 
            context.Message.VerificationCode, 
            EmailSender.EmailTarget.Verification);
    }
}
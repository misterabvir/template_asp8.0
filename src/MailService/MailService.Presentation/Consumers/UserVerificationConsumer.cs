using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Presentation.Consumers;

public class UserVerificationConsumer( 
    ISender sender) : 
    IConsumer<UserVerifiedEvent>
{
    public async Task Consume(ConsumeContext<UserVerifiedEvent> context)
    {
            var command = new MailService.Application.Commands.Verification.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "Account must be verified",
            context.Message.VerificationCode);
        await sender.Send(command);   
    }   
}
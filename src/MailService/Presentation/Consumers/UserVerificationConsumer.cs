using MassTransit;

using MediatR;

using Shared.Events;

namespace Presentation.Consumers;

public class UserVerificationConsumer( 
    ISender sender) : 
    IConsumer<UserVerifiedEvent>
{
    public async Task Consume(ConsumeContext<UserVerifiedEvent> context)
    {
            var command = new Application.Commands.Verification.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "Account suspended",
            context.Message.VerificationCode);
        await sender.Send(command);   
    }   
}

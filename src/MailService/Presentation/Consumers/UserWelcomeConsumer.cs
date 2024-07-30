using MassTransit;
using MediatR;
using Shared.Events;

namespace Presentation.Consumers;

public class UserWelcomeConsumer(
    ISender sender) :
    IConsumer<UserConfirmedEvent>
{
    public async Task Consume(ConsumeContext<UserConfirmedEvent> context)
    {
        var command = new Application.Commands.Welcome.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "Account suspended");
        await sender.Send(command);   
    }
}

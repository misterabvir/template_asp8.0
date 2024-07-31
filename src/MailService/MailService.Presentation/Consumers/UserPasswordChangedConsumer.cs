using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Presentation.Consumers;

public class UserPasswordChangedConsumer(
    ISender sender) :
    IConsumer<UserPasswordChangedEvent>
{
    public async Task Consume(ConsumeContext<UserPasswordChangedEvent> context)
    {
        var command = new MailService.Application.Commands.Warning.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "Password Changed");
        await sender.Send(command);
    }
}


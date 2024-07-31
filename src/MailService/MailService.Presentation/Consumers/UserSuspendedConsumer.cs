using MassTransit;
using MediatR;
using Application.Events;

namespace MailService.Presentation.Consumers;

public class UserSuspendedConsumer(
    ISender sender) :
    IConsumer<UserSuspendedEvent>
{
    public async Task Consume(ConsumeContext<UserSuspendedEvent> context)
    {
        var command = new MailService.Application.Commands.Warning.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "Account suspended");
        await sender.Send(command);     
    }
}


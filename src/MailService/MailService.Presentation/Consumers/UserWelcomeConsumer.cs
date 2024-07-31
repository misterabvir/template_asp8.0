using MassTransit;
using MediatR;
using Shared.Events;
namespace MailService.Presentation.Consumers;

public class UserWelcomeConsumer(
    ISender sender) :
    IConsumer<UserConfirmedEvent>
{
    public async Task Consume(ConsumeContext<UserConfirmedEvent> context)
    {
        var command = new MailService.Application.Commands.Welcome.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            "User confirmed account");
        await sender.Send(command);   
    }
}

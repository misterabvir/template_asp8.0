using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Presentation.Consumers;

public class UserRoleConsumer(
    ISender sender) :
    IConsumer<UserRoleChangedEvent>
{
    public async Task Consume(ConsumeContext<UserRoleChangedEvent> context)
    {
        var command = new MailService.Application.Commands.RoleChanged.Command(
            context.Message.UserId,
            context.Message.Email,
            context.Message.Username,
            $"Role changed to '{context.Message.Role}'",
            context.Message.Role);
        await sender.Send(command);
    }
}

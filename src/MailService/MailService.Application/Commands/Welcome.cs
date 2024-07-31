using MailService.Application.Common.Repositories;
using MailService.Application.Common.Services;

using MailService.Domain;

using MediatR;

namespace MailService.Application.Commands;

public static class Welcome
{
    public record Command(Guid UserId, string Email, string Username,  string Reason) : IRequest;
    public class Handler(IUserRepository userRepository, IMessageRepository messageRepository, IEmailSender emailSender) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetOrCreateAsync(command.UserId, command.Email, command.Username, cancellationToken);
            await emailSender.SendWelcomeEmailAsync(user.Email, user.Username);
            await messageRepository.AddMessageAsync(user.UserId, Constants.Types.Welcome, command.Reason, cancellationToken);
        }
    }
}
using Application.Common.Repositories;
using Application.Common.Services;

using Domain;

using MediatR;

namespace Application.Commands;

public static class Verification
{
    public record Command(Guid UserId, string Email, string Username, string Reason, string Code) : IRequest;
    public class Handler(IUserRepository userRepository, IMessageRepository messageRepository, IEmailSender emailSender) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetOrCreateAsync(command.UserId, command.Email, command.Username, cancellationToken);
            await emailSender.SendVerificationEmailAsync(user.Email, user.Username, command.Code);
            await messageRepository.AddMessageAsync(user.UserId, Constants.Types.Verification, command.Reason , cancellationToken);
        }
    }
}

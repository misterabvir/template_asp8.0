using Domain.UserAggregate.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Application.Users.Events;

public class UserVerifiedNotificationHandler(ILogger<UserVerifiedNotificationHandler> logger) : INotificationHandler<UserVerifiedDomainEvent>
{
    public async Task Handle(UserVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        logger.LogInformation("User {UserId} has been verified", notification.User.Id);
    }
}

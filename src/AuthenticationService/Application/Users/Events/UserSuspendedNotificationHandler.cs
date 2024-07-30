using Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Shared.Events;

namespace Application.Users.Events;

public class UserSuspendedNotificationHandler(
    ILogger<UserSuspendedNotificationHandler> logger, 
    IPublishEndpoint endpoint) : INotificationHandler<UserSuspendedDomainEvent>
{

    public async Task Handle(UserSuspendedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserSuspendedEvent(notification.UserId, notification.Username, notification.Email), cancellationToken);
        logger.LogInformation("User {UserId} account was suspended", notification.UserId);
    }
}
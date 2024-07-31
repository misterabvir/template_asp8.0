using AuthenticationService.Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Shared.Events;

namespace AuthenticationService.Application.Users.Events;

public class UserRoleChangedNotificationHandler(ILogger<UserRoleChangedNotificationHandler> logger, IPublishEndpoint endpoint) : INotificationHandler<UserRoleChangedDomainEvent>
{
    public async Task Handle(UserRoleChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserRoleChangedEvent(notification.UserId, notification.Username, notification.Email, notification.Role), cancellationToken);
        logger.LogInformation("User {UserId} has role {Role} changed", notification.UserId, notification.Role);
    }
}

using AuthenticationService.Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Application.Events;

namespace AuthenticationService.Application.Users.Events;

public class UserUsernameChangedNotificationHandler(ILogger<UserUsernameUpdatedDomainEvent> logger, IPublishEndpoint publishEndpoint) : INotificationHandler<UserUsernameUpdatedDomainEvent>
{
    public async Task Handle(UserUsernameUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new UserAccountDataChangedEvent(
            notification.UserId,
            notification.Username,
            notification.Avatar
        ), cancellationToken);
        logger.LogInformation("User {UserId} username has been changed", notification.UserId);
    }
}


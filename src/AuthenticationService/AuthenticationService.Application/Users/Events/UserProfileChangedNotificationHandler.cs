using AuthenticationService.Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Application.Events;

namespace AuthenticationService.Application.Users.Events;

public class UserProfileChangedNotificationHandler(
     ILogger<UserProfileUpdatedDomainEvent> logger,
     IPublishEndpoint publishEndpoint) : INotificationHandler<UserProfileUpdatedDomainEvent>
{
    public async Task Handle(UserProfileUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new UserAccountDataChangedEvent(
            notification.UserId,
            notification.Username,
            notification.Avatar
        ), cancellationToken);
        logger.LogInformation("User {UserId} profile has been changed", notification.UserId);
    }
}


using AuthenticationService.Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Shared.Events;

namespace AuthenticationService.Application.Users.Events;

public class UserPasswordChangedNotificationHandler(
    ILogger<UserPasswordChangedNotificationHandler> logger, 
    IPublishEndpoint endpoint) : INotificationHandler<UserPasswordChangedDomainEvent>
{

    public async Task Handle(UserPasswordChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserPasswordChangedEvent(notification.UserId, notification.Username, notification.Email), cancellationToken);
        logger.LogInformation("User {UserId} has changed password", notification.UserId);
    }
}

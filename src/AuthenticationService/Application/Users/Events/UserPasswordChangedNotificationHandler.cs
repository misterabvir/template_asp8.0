using Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Shared.Events;

namespace Application.Users.Events;

public class UserPasswordChangedNotificationHandler(
    ILogger<UserPasswordChangedNotificationHandler> logger, 
    IPublishEndpoint endpoint) : INotificationHandler<UserPasswordChangedDomainEvent>
{

    public async Task Handle(UserPasswordChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserWarningActivityEvent(notification.Username, notification.Email), cancellationToken);
        logger.LogInformation("User {UserId} has changed password", notification.UserId);
    }
}

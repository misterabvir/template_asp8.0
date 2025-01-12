using AuthenticationService.Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Application.Events;

namespace AuthenticationService.Application.Users.Events;

public class UserVerifiedNotificationHandler(ILogger<UserVerifiedNotificationHandler> logger, IPublishEndpoint endpoint) : INotificationHandler<UserVerifiedDomainEvent>
{
    public async Task Handle(UserVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserSuccessVerifiedEvent(notification.UserId, notification.Username, notification.Email), cancellationToken);
        logger.LogInformation("User {UserId} has been verified", notification.UserId);
    }
}

using Domain.UserAggregate.Events;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Logging;

using Shared.Events;

namespace Application.Users.Events;

public class UserVerifiedNotificationHandler(ILogger<UserVerifiedNotificationHandler> logger, IPublishEndpoint endpoint) : INotificationHandler<UserVerifiedDomainEvent>
{
    public async Task Handle(UserVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        await endpoint.Publish(new UserWelcomeEmailSentEvent(notification.Username, notification.Email), cancellationToken);
        logger.LogInformation("User {UserId} has been verified", notification.UserId);
    }
}

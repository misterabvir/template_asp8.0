
using Application.Common.Services;

using Domain.UserAggregate.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Application.Users.Events;

public class UserCreatedNotificationHandler(IVerificationService verificationService, ILogger<UserCreatedNotificationHandler> logger) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await verificationService.SendVerificationCodeAsync(notification.User, cancellationToken);
        logger.LogInformation("User {UserId} has been created", notification.User.Id);
    }
}

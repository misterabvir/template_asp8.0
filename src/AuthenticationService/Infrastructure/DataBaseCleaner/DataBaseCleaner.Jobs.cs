using Domain.Persistence.Contexts;
using Domain.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Infrastructure.DataBaseCleaner;

public static partial class DataBaseCleaner
{
    [DisallowConcurrentExecution]
    public class CleanerJob(AuthenticationDbContext dbContext, ILogger<CleanerJob> logger, Settings settings) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var users = await dbContext.Users
            .Where(x => x.Status == Status.NotVerified && (DateTime.UtcNow - x.CreatedAt).TotalHours > settings.NotVerifiedTimeLimitInHours)
            .ToListAsync(context.CancellationToken);

            foreach (var user in users)
            {
                dbContext.Users.Remove(user);
                logger.LogInformation("User {Id} removed", user.Id);
            }

            var oldMessages = await dbContext.OutboxMessages
                .Where(x => (DateTime.UtcNow - x.CreatedAt).TotalDays > settings.OldMessagesTimeLimitInDays)
                .ToListAsync(context.CancellationToken);

            foreach (var message in oldMessages)
            {
                dbContext.OutboxMessages.Remove(message);
                logger.LogInformation("Message {Id} removed", message.OutboxMessageId);
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
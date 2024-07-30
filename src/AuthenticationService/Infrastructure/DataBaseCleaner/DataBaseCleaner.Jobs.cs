using Domain.UserAggregate.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure;

public static partial class DataBaseCleaner
{
    [DisallowConcurrentExecution]
    public class CleanerJob(AuthenticationDbContext dbContext, Settings settings) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var users = await dbContext.Users
            .Where(x => x.Status == Status.NotVerified && (DateTime.UtcNow - x.CreatedAt).TotalHours > settings.NotVerifiedTimeLimitInHours)
            .ToListAsync(context.CancellationToken);

            foreach (var user in users)
            {
                dbContext.Users.Remove(user);
            }

            var oldMessages = await dbContext.OutboxMessages
                .Where(x => (DateTime.UtcNow - x.CreatedAt).TotalDays > settings.OldMessagesTimeLimitInDays)
                .ToListAsync(context.CancellationToken);

            foreach (var message in oldMessages)
            {
                dbContext.OutboxMessages.Remove(message);
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
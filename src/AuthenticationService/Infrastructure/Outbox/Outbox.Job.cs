using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Microsoft.Extensions.Logging;
using Domain.UserAggregate.Events;
using MediatR;
using Infrastructure.Persistence.Contexts;
namespace Infrastructure;

public static partial class Outbox
{
 
    [DisallowConcurrentExecution]
    public class PublishJob(AuthenticationDbContext dbContext, IPublisher publisher, ILogger<PublishJob> logger, Settings settings) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(settings.MessagePerOneTime)
            .ToListAsync(context.CancellationToken);
            foreach (var message in messages)
            {               
                if (!RegisteredDomainEvents.Events.TryGetValue(message.Type, out var eventType))
                {
                    continue;
                }
                var domainEvent = JsonConvert.DeserializeObject(message.Content, eventType);
                if (domainEvent is null)
                {
                    continue;
                }
                await publisher.Publish(domainEvent);
                message.ProcessedAt = DateTime.UtcNow;
                logger.LogInformation("Message {Type} has been published.", message.Type);
            }
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
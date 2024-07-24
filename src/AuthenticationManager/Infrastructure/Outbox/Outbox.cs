using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Newtonsoft.Json;

using Domain.Persistence.Contexts;

using Quartz;

using Shared.Domain;
namespace Infrastructure;

public static class Outbox
{
    const string JobKey = "OutboxMessagesJob";
    const int IntervalInSeconds = 10;
    const int MessagePerOneTime = 20;
    
    public static IServiceCollection AddOutboxMessages(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(options =>
        {
            
            var jobKey = new JobKey(JobKey);
            options.AddJob<PublishJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(IntervalInSeconds).RepeatForever()));
        });
        return services;
    }

    [DisallowConcurrentExecution]
    public class PublishJob(AuthenticationDbContext dbContext, IPublisher publisher) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await dbContext.OutboxMessages
            .Where(x => !x.IsProcessed)
            .Take(MessagePerOneTime)
            .ToListAsync(context.CancellationToken);
            foreach (var message in messages)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content);
                if (domainEvent is null)
                {
                    continue;
                }
                await publisher.Publish(domainEvent);
                message.ProcessedAt = DateTime.UtcNow;
            }
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
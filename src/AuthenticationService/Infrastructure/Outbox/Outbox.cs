using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Newtonsoft.Json;
using Domain.Persistence.Contexts;
using Quartz;
using Microsoft.Extensions.Logging;
using Domain.UserAggregate.Events;
namespace Infrastructure;

public static class Outbox
{
    const string JobKey = "OutboxMessagesJob";
    const int IntervalInSeconds = 20;
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
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }

    [DisallowConcurrentExecution]
    public class PublishJob(AuthenticationDbContext dbContext, IPublisher publisher, ILogger<PublishJob> logger) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null)
            .Take(MessagePerOneTime)
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
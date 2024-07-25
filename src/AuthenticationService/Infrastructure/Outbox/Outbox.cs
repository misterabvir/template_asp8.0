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

    public static Settings AddOutboxMessages(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? 
            throw new Exception("Outbox settings not configured");
        services.AddSingleton(settings);
        return settings;
    }

    public static void AddOutboxMessagesJob(this IServiceCollectionQuartzConfigurator configurator, Settings settings )
    {
        var jobKey = new JobKey(settings.JobKey);
            configurator.AddJob<PublishJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(settings.IntervalInSeconds).RepeatForever()));
    }
    public class Settings{
        public const string SectionName = "Settings:Outbox";
        public required string JobKey { get; set; } 
        public required int IntervalInSeconds { get; set; }
        public required int MessagePerOneTime { get; set; }
    }


    [DisallowConcurrentExecution]
    public class PublishJob(AuthenticationDbContext dbContext, IPublisher publisher, ILogger<PublishJob> logger, Settings settings) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null)
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
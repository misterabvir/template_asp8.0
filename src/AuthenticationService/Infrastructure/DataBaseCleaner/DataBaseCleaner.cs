using Domain.Persistence.Contexts;
using Domain.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Infrastructure.DataBaseCleaner;

public static class DataBaseCleaner
{
    public static Settings AddDataBaseCleaner(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>()
        ?? throw new Exception("DataBaseCleaner settings not configured");
        services.AddSingleton(settings);
        return settings;
    }
    public static void AddDataBaseCleanerJob(this IServiceCollectionQuartzConfigurator configurator, Settings settings )
    {
        var jobKey = new JobKey(settings.JobKey);
            configurator.AddJob<CleanerJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(settings.IntervalInSeconds).RepeatForever()));
    }
    
    
    public class Settings
    {
        public const string SectionName = "Settings:DataBaseCleaner";
        public required string JobKey { get; set; }
        public required int NotVerifiedTimeLimitInHours { get; set; } = 24;
        public required int OldMessagesTimeLimitInDays { get; set; } = 7;
        public required int IntervalInSeconds { get; set; } = 3600;
    }


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
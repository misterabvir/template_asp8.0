using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
namespace AuthenticationService.Infrastructure.BackgroundJobs;

public static partial class Outbox
{
    public static Settings AddOutboxMessages(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? 
            throw new OutboxSettingsNotConfiguredException();
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
}
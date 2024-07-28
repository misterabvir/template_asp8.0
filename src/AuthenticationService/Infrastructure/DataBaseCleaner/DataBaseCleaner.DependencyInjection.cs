using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Infrastructure;

public static partial class DataBaseCleaner
{
    public static Settings AddDataBaseCleaner(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>()
        ?? throw new DatabaseCleanerSettingsNotConfiguredException();
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
}
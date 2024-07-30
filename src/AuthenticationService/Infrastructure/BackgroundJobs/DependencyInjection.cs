using Application.Common.Repositories;

using Infrastructure.Persistence;
using Infrastructure.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Infrastructure.BackgroundJobs;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var outboxSettings = services.AddOutboxMessages(configuration);
        var cleanerSettings = services.AddDataBaseCleaner(configuration);
        
        services.AddQuartz(options =>
        {      
           options.AddOutboxMessagesJob(outboxSettings);
           options.AddDataBaseCleanerJob(cleanerSettings);
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
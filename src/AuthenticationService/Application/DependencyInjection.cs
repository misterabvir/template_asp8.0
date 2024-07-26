using Application.Behaviors;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
   private const string CacheConnection = "RedisConnection";
   private const string QueueConnection = "QueueConnection";
   private const string InstanceName = "RedisCache";
    
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionCacheString = configuration.GetConnectionString(CacheConnection) ??
                throw new CacheConnectionNotConfiguredException();
        var connectionQueueString = configuration.GetConnectionString(QueueConnection) ??
                throw new QueueConnectionNotConfiguredException(); 

        services
            .AddMediator()
            .AddCaching(connectionCacheString)
            .AddQueue(connectionQueueString);
        
        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));        return services;
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = InstanceName;
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { connectionString }
            };
        });

        return services;
    }

    private static IServiceCollection AddQueue(this IServiceCollection services, string connectionString)
    {
        services.AddMassTransit(options => options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(connectionString);
                cfg.ConfigureEndpoints(context);
            }));

        return services;
    }


    public class QueueConnectionNotConfiguredException : Exception
    {
        public QueueConnectionNotConfiguredException() : base("Queue connection string not configured")
        {
        }
    }

    public class CacheConnectionNotConfiguredException : Exception
    {
        public CacheConnectionNotConfiguredException() : base("Cache connection string not configured")
        {
        }
    }

}
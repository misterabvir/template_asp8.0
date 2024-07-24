using Application.Behaviors;

using FluentValidation;

using MassTransit;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddStackExchangeRedisCache(options =>
        {
            var connectionCacheString = configuration.GetConnectionString("RedisConnection") ??
                throw new Exception("Redis connection string not found");

            options.Configuration = configuration.GetConnectionString(connectionCacheString);
            options.InstanceName = "RedisCache";
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { connectionCacheString }
            };
        });
        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("QueueConnection") ?? 
                    throw new Exception("Queue connection string not configured"));
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }

}
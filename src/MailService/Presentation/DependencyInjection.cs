using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Presentation.Consumers;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services, 
        IConfiguration configuration)
    {

        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("QueueConnection") ??
                    throw new Exception("Queue connection string not configured"));
                cfg.ConfigureEndpoints(context);
            });
            options.AddConsumer<UserVerificationConsumer>();
            options.AddConsumer<UserWelcomeConsumer>();
            options.AddConsumer<UserPasswordChangedConsumer>();
            options.AddConsumer<UserRoleConsumer>();
            options.AddConsumer<UserSuspendedConsumer>();
        });

        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        return app;
    }
}

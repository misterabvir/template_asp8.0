using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailService.Infrastructure.Consumers;

public static class DependencyInjection
{
    public const string QueueConnection = "QueueConnection";

    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString(QueueConnection) ?? throw new QueueConnectionNotConfiguredException());
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

    public class QueueConnectionNotConfiguredException : Exception
    {
        public QueueConnectionNotConfiguredException() : base("Queue connection not configured")
        {
        }
    }
}
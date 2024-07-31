using MassTransit;

using MailService.Presentation.Common;
using MailService.Presentation.Common.Exceptions;

namespace MailService.Presentation.Consumers;

public static class DependencyInjection
{
    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString(Constants.QueueConnection) ?? throw new QueueConnectionNotConfiguredException());
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
}
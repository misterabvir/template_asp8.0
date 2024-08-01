using MailService.Infrastructure.Consumers;
using MailService.Infrastructure.Persistence;
using MailService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddConsumers(configuration);
        services.AddTokenAuthorization(configuration);
        services.AddPersistence(configuration);
        services.AddEmailSender(configuration);
        services.AddRepositories();
        return services;
    }
}

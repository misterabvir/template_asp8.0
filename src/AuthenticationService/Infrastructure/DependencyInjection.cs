using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTokens(configuration)
            .AddEncrypts(configuration)
            .AddVerifications(configuration)
            .AddOutboxMessages(configuration);
        return services;
    }

}
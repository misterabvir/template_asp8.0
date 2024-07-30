using Application.Common.Repositories;

using Infrastructure.Persistence;
using Infrastructure.Repositories;

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionDbString = configuration.GetConnectionString("DbConnection") ?? 
            throw new Exception("DbConnectionString is not configured");
        services.AddTransient(p=> new DbConnectionFactory(connectionDbString));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<TemplateRepository>();
        services.AddEmailSender(configuration);


        return services;
    }
}

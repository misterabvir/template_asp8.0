using MailService.Application.Common.Repositories;
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
        var connectionDbString = configuration.GetConnectionString("DbConnection") ?? 
            throw new Exception("DbConnectionString is not configured");
        services.AddSingleton(p=> new DbConnectionFactory(connectionDbString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<TemplateRepository>();
        services.AddEmailSender(configuration);


        return services;
    }
}

using DbUp;
using System.Reflection;

using Infrastructure;

using MailService.Infrastructure.Consumers;
using MailService.Infrastructure.Persistence;
using MailService.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
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
        services.AddTokenAuthorization();
        services.AddPersistence(configuration);
        services.AddEmailSender(configuration);
        services.AddRepositories();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var scope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();

        var context = scope.ServiceProvider.GetService<DbConnectionFactory>() ??
            throw new Exception("DbConnectionFactory is not exist");

        EnsureDatabase.For.MySqlDatabase(context.ConnectionString);

        var upgrader = DeployChanges.To
            .MySqlDatabase(context.ConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        upgrader.PerformUpgrade();
        return app;
    }
}
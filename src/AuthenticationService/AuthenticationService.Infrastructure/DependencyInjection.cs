using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Infrastructure.BackgroundJobs;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddRepositories()
            .AddTokens(configuration)
            .AddEncrypts(configuration)
            .AddVerifications(configuration)
            .AddBackgroundJobs(configuration)
            ;
        return services;
    }



    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}